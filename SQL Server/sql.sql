-- Tạo bảng tblUser với cột id tự động tăng
CREATE TABLE tblUser (
    Id INT IDENTITY PRIMARY KEY,
    sEmail VARCHAR(255) NOT NULL,
    sPasswordHash VARCHAR(255) NOT NULL,
	sSalt VARCHAR(255) NOT NULL,
    sRole VARCHAR(50)
);

-- Tạo bảng tblLanguage với cột id tự động tăng
CREATE TABLE tblLanguage (
    Id INT IDENTITY PRIMARY KEY,
    sLanguage NVARCHAR(50) NOT NULL
);

-- Tạo bảng tblWord_type với cột id tự động tăng
CREATE TABLE tblWord_type (
    Id INT IDENTITY PRIMARY KEY,
    sWordtype NVARCHAR(50) NOT NULL
);

-- Tạo bảng tblWord với cột id tự động tăng
CREATE TABLE tblWord (
    Id INT IDENTITY PRIMARY KEY,
    Id_Language INT,
	Id_Language_trans INT,
    Id_wordtype INT,
    Id_user INT,
    sWord NVARCHAR(255) NOT NULL,
    sExample  NVARCHAR(255),
    sDefinition  NVARCHAR(255),
    FOREIGN KEY (Id_Language) REFERENCES tblLanguage(Id),
	FOREIGN KEY (Id_Language_trans) REFERENCES tblLanguage(Id),
    FOREIGN KEY (Id_wordtype) REFERENCES tblWord_type(Id),
    FOREIGN KEY (Id_user) REFERENCES tblUser(Id)
);

-- Tạo bảng tblHistory_search với cột id tự động tăng
CREATE TABLE tblHistory_search (
    Id INT IDENTITY PRIMARY KEY,
    Id_user INT,
    Id_word INT,
    dDatetime DATETIME,
    FOREIGN KEY (Id_user) REFERENCES tblUser(Id),
    FOREIGN KEY (Id_word) REFERENCES tblWord(Id)
);

-- Tạo bảng tblHistory_add với cột id tự động tăng
CREATE TABLE tblHistory_add (
    Id INT IDENTITY PRIMARY KEY,
    Id_user INT,
    Id_word INT,
    dDatetime DATETIME,
    FOREIGN KEY (Id_user) REFERENCES tblUser(Id),
    FOREIGN KEY (Id_word) REFERENCES tblWord(Id)
);

Alter  PROCEDURE SearchWords
    @word NVARCHAR(255),
    @lang NVARCHAR(50),
    @lang_trans NVARCHAR(50)
AS
BEGIN
    SELECT w.sWord, wt.sWordtype, w.sExample, w.sDefinition,w.Id
    FROM tblWord w
    INNER JOIN tblWord_type wt ON w.Id_wordtype = wt.Id
    INNER JOIN tblLanguage lang ON w.Id_Language = lang.Id
    INNER JOIN tblLanguage lang_trans ON w.Id_Language_trans = lang_trans.Id
    WHERE (LOWER(lang.sLanguage) = LOWER(@lang) OR (@lang = @lang_trans AND LOWER(lang.sLanguage) = LOWER(@lang_trans)))
          AND LOWER(w.sWord) LIKE LOWER(@word) + '%';
END


Insert into tblLanguage 
values('English'),('VietNamese');

Insert into tblWord_type 
values(N'Động từ'),(N'Danh từ'),(N'Tính từ');

-- thêm 1 user để thêm word
Insert into tblUser 
values (N'mochimochipo1122@gmail.com',N'sdfgadfg',N'asdfasdfasdf',N'Admin')

INSERT INTO tblWord (Id_Language,Id_Language_trans, Id_wordtype, Id_user, sWord, sExample, sDefinition)
VALUES 
(1, 2, 2, 1, N'Book', N'This is a book', N'A physical or digital document consisting of pages bound together'),
(1, 2, 1, 1, N'Run', N'He runs every morning', N'To move swiftly on foot'),
(2, 1, 3, 1, N'Đẹp', N'Cô ấy rất đẹp', N'Gợi cảm, hấp dẫn'),
(1, 2, 2, 1, N'Computer', N'I use a computer for work', N'An electronic device for storing and processing data'),
(2, 1, 1, 1, N'Học', N'Tôi đang học tiếng Anh', N'Hành động học hoặc nắm vững kiến thức'),
(1, 2, 3, 1, N'Happy', N'I am very happy today', N'Feeling or showing pleasure or contentment');



--- bắt đầu sửa từ đây cập nhật lần cuối 3/12/2023

Alter PROCEDURE GetWordInfoById
    @WordId INT
AS
BEGIN
    SELECT
        w.Id AS WordId,
        w.sWord,
        w.sDefinition,
        w.sExample,
        l.sLanguage,
        wt.sWordtype
    FROM
        tblWord w
    INNER JOIN
        tblLanguage l ON w.Id_Language = l.Id
    INNER JOIN
        tblWord_type wt ON w.Id_wordtype = wt.Id
    WHERE
        w.Id = @WordId;
END;


CREATE PROCEDURE InsertOrUpdateHistorySearch
    @Id_user INT,
    @Id_word INT,
    @dDatetime DATETIME
AS
BEGIN
    -- Kiểm tra xem bản ghi có tồn tại không
    IF EXISTS (SELECT 1 FROM tblHistory_search WHERE Id_user = @Id_user AND Id_word = @Id_word)
    BEGIN
        -- Bản ghi tồn tại, cập nhật dDatetime
        UPDATE tblHistory_search
        SET dDatetime = @dDatetime
        WHERE Id_user = @Id_user AND Id_word = @Id_word;
    END
    ELSE
    BEGIN
        -- Bản ghi không tồn tại, thêm mới
        INSERT INTO tblHistory_search (Id_user, Id_word, dDatetime)
        VALUES (@Id_user, @Id_word, @dDatetime);
    END
END;


Alter PROCEDURE GetWordsByUserId
    @Id_user INT
AS
BEGIN
    SELECT
        w.Id AS WordId,
        w.sWord,
        w.sDefinition,
        w.sExample,
        l.sLanguage AS OriginalLanguage,
        wt.sWordtype,
        w.Id_Language_trans AS Id_Language_trans,
        lt.sLanguage AS TranslationLanguage,
        hs.dDatetime,
        hs.Id_user AS HistoryUserId
    FROM
        tblWord w
    INNER JOIN
        tblLanguage l ON w.Id_Language = l.Id
    INNER JOIN
        tblWord_type wt ON w.Id_wordtype = wt.Id
    LEFT JOIN
        tblLanguage lt ON w.Id_Language_trans = lt.Id
    LEFT JOIN
        tblHistory_search hs ON w.Id = hs.Id_word AND hs.Id_user = @Id_user
    WHERE
        w.Id_user = @Id_user
        AND hs.Id_user IS NOT NULL
    ORDER BY
        hs.dDatetime DESC; -- Sắp xếp theo dDatetime giảm dần
END;



CREATE PROCEDURE sp_DeleteHistorySearch
    @UserId INT,
    @WordId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM tblHistory_search
    WHERE Id_user = @UserId
        AND Id_word = @WordId;
END;