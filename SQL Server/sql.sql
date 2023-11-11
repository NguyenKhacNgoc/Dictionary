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

CREATE PROCEDURE SearchWords
    @_word NVARCHAR(255),
    @_lang NVARCHAR(50),
    @_lang_trans NVARCHAR(50)
AS
BEGIN
    SELECT w.sWord, wt.sWordtype, w.sExample, w.sDefinition
    FROM tblWord w
    INNER JOIN tblWord_type wt ON w.Id_wordtype = wt.Id
    INNER JOIN tblLanguage lang ON w.Id_Language = lang.Id
    INNER JOIN tblLanguage lang_trans ON w.Id_Language_trans = lang_trans.Id
    WHERE (LOWER(lang.sLanguage) = LOWER(@_lang) OR (@_lang = @_lang_trans AND LOWER(lang.sLanguage) = LOWER(@_lang_trans)))
          AND LOWER(w.sWord) LIKE LOWER(@_word) + '%';
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
