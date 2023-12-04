CREATE DATABASE Dictionary 

USE Dictionary

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

DROP TABLE tblWord;


-- Tạo bảng tblHistory_search với cột id tự động tăng
CREATE TABLE tblHistory_search (
    Id INT IDENTITY PRIMARY KEY,
    Id_user INT,
    Id_word INT,
    dDatetime DATETIME,
    FOREIGN KEY (Id_user) REFERENCES tblUser(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (Id_word) REFERENCES tblWord(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Tạo bảng tblHistory_add với cột id tự động tăng
CREATE TABLE tblHistory_add (
    Id INT IDENTITY PRIMARY KEY,
    Id_user INT,
    Id_word INT,
    dDatetime DATETIME,
    FOREIGN KEY (Id_user) REFERENCES tblUser(Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (Id_word) REFERENCES tblWord(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE  PROCEDURE SearchWords
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
values(N'Động từ'),(N'Danh từ'),(N'Tính từ'),(N'Trạng từ');

-- thêm 1 user để thêm word
Insert into tblUser 
values (N'mochimochipo1122@gmail.com',N'sdfgadfg',N'asdfasdfasdf',N'Admin')
-- Từ điển tiếng anh 
INSERT INTO tblWord (Id_Language, Id_Language_trans, Id_wordtype, Id_user, sWord, sExample, sDefinition)
VALUES 
(1, 2, 2, 1, N'Book', N'This is a book', N'Một tài liệu vật lý hoặc kỹ thuật số bao gồm các trang được liên kết với nhau'),
(1, 2, 1, 1, N'Run', N'He runs every morning', N'Anh ấy chạy vào mỗi buổi sáng'),
(2, 1, 3, 1, N'Đẹp', N'Cô ấy rất đẹp', N'She is very beautiful'),
(1, 2, 2, 1, N'Apple', N'This apple is delicious', N'Quả táo'),
(1, 2, 3, 1, N'Beautiful', N'He is a beautiful girl', N'Xinh đẹp'),
(1, 2, 3, 1, N'Cat', N'The cat is sleeping', N'Con mèo'),
(1, 2, 2, 1, N'Orange', N'I like to eat oranges', N'Tôi thích ăn cam'),
(1, 2, 3, 1, N'Playful', N'The puppy is very playful', N'Con chó con rất nghịch ngợm'),
(1, 2, 3, 1, N'Quiet', N'Please be quiet in the library', N'Vui lòng giữ yên tĩnh trong thư viện'),
(1, 2, 1, 1, N'Read', N'She loves to read books', N'Cô ấy thích đọc sách'),
(1, 2, 2, 1, N'Sun', N'The sun is shining brightly', N'Mặt trời đang chiếu sáng rực rỡ'),
(1, 2, 3, 1, N'Tree', N'I planted a tree in my backyard', N'Tôi trồng một cây trong sân sau của mình'),
(1, 2, 2, 1, N'Umbrella', N'Take an umbrella, it might rain', N'Mang theo cái ô, có thể sẽ mưa'),
(1, 2, 1, 1, N'Very', N'The movie was very interesting', N'Bộ phim rất thú vị'),
(1, 2, 2, 1, N'Water', N'Drink plenty of water', N'Hãy uống nhiều nước'),
(1, 2, 3, 1, N'Xylophone', N'The xylophone produces beautiful music', N'Kén xylophone tạo ra âm nhạc đẹp'),
(1, 2, 2, 1, N'Yellow', N'The sunflower is yellow', N'Hoa hướng dương màu vàng'),
(1, 2, 3, 1, N'Zoo', N'We visited the zoo last weekend', N'Chúng tôi đã thăm sở thú cuối tuần trước'),
(1, 2, 1, 1, N'Exercise', N'Daily exercise is good for health', N'Tập thể dục hàng ngày tốt cho sức khỏe'),
(1, 2, 3, 1, N'Giraffe', N'The giraffe has a long neck', N'Hươu cao cổ có cổ dài'),
(1, 2, 2, 1, N'Kite', N'Flying a kite in the park', N'Bay cánh diều ở công viên'),
(1, 2, 2, 1, N'Juice', N'I like to drink orange juice', N'Tôi thích uống nước cam'),
(1, 2, 1, 1, N'Laugh', N'The joke made everyone laugh', N'Câu đùa khiến mọi người cười'),
(2, 1, 3, 1, N'Hạnh phúc', N'Cả gia đình họ sống trong hạnh phúc', N'The whole family lives in happiness'),
(1, 2, 2, 1, N'Mountain', N'The mountain peak was covered in snow', N'Đỉnh núi được phủ tuyết'),
(1, 2, 3, 1, N'Nature', N'I love spending time in nature', N'Tôi thích dành thời gian ở trong thiên nhiên'),
(1, 2, 3, 1, N'Ocean', N'The ocean waves were mesmerizing', N'Những con sóng biển làm say mê'),
(1, 2, 2, 1, N'Piano', N'She plays the piano beautifully', N'Cô ấy chơi đàn piano đẹp'),
(1, 2, 3, 1, N'Quiet', N'Enjoy the quiet moments in life', N'Hãy tận hưởng những khoảnh khắc yên bình trong cuộc sống'),
(1, 2, 1, 1, N'Relax', N'Take a break and relax for a while', N'Nghỉ ngơi một chút và thư giãn'),
(1, 2, 2, 1, N'Smile', N'Her smile brightened up the room', N'Nụ cười của cô ấy làm sáng lên căn phòng'),
(1, 2, 3, 1, N'Tree', N'Trees provide shade and oxygen', N'Cây cung cấp bóng mát và oxy'),
(1, 2, 2, 1, N'Umbrella', N'Carry an umbrella in case it rains', N'Mang theo cái ô trường hợp mưa'),
(1, 2, 1, 1, N'Vivid', N'The sunset painted the sky in vivid colors', N'Hoàng hôn tô màu bầu trời rực rỡ'),
(1, 2, 2, 1, N'Whale', N'We saw a whale during our boat trip', N'Chúng tôi thấy một con cá voi trong chuyến đi thuyền'),
(1, 2, 3, 1, N'Xylophone', N'Playing the xylophone requires precision', N'Chơi kén xylophone đòi hỏi sự chính xác'),
(1, 2, 2, 1, N'Yellow', N'She wore a bright yellow dress', N'Cô ấy mặc một chiếc váy màu vàng sáng'),
(1, 2, 3, 1, N'Zest', N'Cooking with zest adds flavor to dishes', N'Nấu ăn với sự hứng thú thêm hương vị cho món ăn'),
(1, 2, 1, 1, N'Adventure', N'Traveling is an exciting adventure', N'Du lịch là một cuộc phiêu lưu thú vị'),
(1, 2, 3, 1, N'Gentle', N'A gentle breeze rustled the leaves', N'Một làn gió nhẹ làm lay động lá cây'),
(1, 2, 2, 1, N'Kangaroo', N'Kangaroos are native to Australia', N'Kangaroo là loài đặc hữu của Australia');


-- Từ điển tiếng việt
INSERT INTO tblWord (Id_Language, Id_Language_trans, Id_wordtype, Id_user, sWord, sExample, sDefinition)
VALUES 
(2, 1, 2, 1, N'Sách', N'Dây là một quyển sách', N'A physical or digital document consisting of pages linked together'),
(2, 1, 1, 1, N'Chạy', N'Anh ấy chạy mỗi buổi sáng', N'He runs every morning'),
(1, 2, 3, 1, N'Đẹp', N'Cô ấy rất đẹp', N'She is very beautiful'),
(2, 1, 2, 1, N'Mâm cơm', N'Mâm cơm này thơm ngon', N'This meal is delicious'),
(2, 1, 3, 1, N'Xinh đẹp', N'Anh ấy là một cô gái xinh đẹp', N'Beautiful'),
(2, 1, 3, 1, N'Con mèo', N'Con mèo đang ngủ', N'The cat is sleeping'),
(2, 1, 2, 1, N'Cam', N'Tôi thích ăn cam', N'I like to eat oranges'),
(2, 1, 3, 1, N'Nghịch ngợm', N'Con chó con rất nghịch ngợm', N'The puppy is very playful'),
(2, 1, 3, 1, N'Yên tĩnh', N'Vui lòng giữ yên tĩnh trong thư viện', N'Please be quiet in the library'),
(2, 1, 1, 1, N'Đọc', N'Cô ấy thích đọc sách', N'She loves to read books'),
(2, 1, 2, 1, N'Mặt trời', N'Mặt trời đang chiếu sáng rực rỡ', N'The sun is shining brightly'),
(2, 1, 3, 1, N'Cây', N'Tôi trồng một cây trong sân sau của mình', N'I planted a tree in my backyard'),
(2, 1, 2, 1, N'Ô', N'Mang theo cái ô, có thể sẽ mưa', N'Take an umbrella, it might rain'),
(2, 1, 1, 1, N'Rất', N'Bộ phim rất thú vị', N'The movie was very interesting'),
(2, 1, 2, 1, N'Nước', N'Hãy uống nhiều nước', N'Drink plenty of water'),
(2, 1, 3, 1, N'Kén xylophone', N'Kén xylophone tạo ra âm nhạc đẹp', N'The xylophone produces beautiful music'),
(2, 1, 2, 1, N'Màu vàng', N'Hoa hướng dương màu vàng', N'The sunflower is yellow'),
(2, 1, 3, 1, N'Sở thú', N'Chúng tôi đã thăm sở thú cuối tuần trước', N'We visited the zoo last weekend'),
(2, 1, 1, 1, N'Tập thể dục', N'Tập thể dục hàng ngày tốt cho sức khỏe', N'Daily exercise is good for health'),
(2, 1, 3, 1, N'Hươu cao cổ', N'Hươu cao cổ có cổ dài', N'The giraffe has a long neck'),
(2, 1, 2, 1, N'Cánh diều', N'Bay cánh diều ở công viên', N'Flying a kite in the park'),
(2, 1, 3, 1, N'Mang theo cái ô', N'Mang theo cái ô, có thể sẽ mưa', N'Take an umbrella, it might rain'),
(2, 1, 2, 1, N'Kéo', N'Tôi đang kéo chiếc xe', N'I am pulling the car'),
(2, 1, 1, 1, N'Ngủ', N'Cô ấy đang ngủ', N'She is sleeping'),
(1, 2, 3, 1, N'Bánh mì', N'Tôi muốn một ổ bánh mì', N'I want a loaf of bread'),
(2, 1, 2, 1, N'Lạnh', N'Ngày hôm nay thời tiết rất lạnh', N'Today the weather is very cold'),
(2, 1, 3, 1, N'Vui', N'Chúng tôi có một buổi tiệc vui vẻ', N'We had a joyful party'),
(2, 1, 3, 1, N'Trường học', N'Trường học này rất lớn', N'This school is very big'),
(2, 1, 2, 1, N'Bút mực', N'Tôi cần một cây bút mực', N'I need a fountain pen'),
(2, 1, 3, 1, N'Đỏ', N'Đoan trang đang mặc chiếc váy màu đỏ', N'Doan Trang is wearing a red dress'),
(2, 1, 1, 1, N'Ăn', N'Chúng ta hãy đi ăn tối', N'Lets go out for dinner'),
(2, 1, 2, 1, N'Máy tính', N'Tôi sử dụng máy tính hàng ngày', N'I use a computer every day'),
(2, 1, 3, 1, N'Biển', N'Chúng ta hãy đi đến bờ biển', N'Lets go to the beach'),
(2, 1, 2, 1, N'Điện thoại', N'Tôi đã để quên điện thoại ở nhà', N'I left my phone at home'),
(2, 1, 1, 1, N'Học', N'Cô ấy đang học tiếng Pháp', N'She is learning French'),
(2, 1, 2, 1, N'Bóng đá', N'Tôi thích xem bóng đá', N'I enjoy watching football'),
(2, 1, 3, 1, N'Bé', N'Em bé đang cười', N'The baby is laughing'),
(2, 1, 3, 1, N'Sân bay', N'Tôi sẽ đón bạn ở sân bay', N'I will pick you up at the airport'),
(2, 1, 2, 1, N'Xe hơi', N'Tôi muốn mua một chiếc xe hơi mới', N'I want to buy a new car'),
(2, 1, 3, 1, N'Đẹp trai', N'Anh chàng này rất đẹp trai', N'This guy is very handsome'),
(2, 1, 2, 1, N'Bàn ăn', N'Chúng ta hãy ngồi vào bàn ăn', N'Lets sit at the dining table'),
(2, 1, 1, 1, N'Nói', N'Đừng nói nữa, hãy nghe tôi nói', N'Dont speak anymore, listen to me');

SELECT * FROM tblWord
SELECT * FROM tblUser
SELECT * FROM tblHistory_search
SELECT * FROM tblLanguage

--- bắt đầu sửa từ đây cập nhật lần cuối 3/12/2023

CREATE PROCEDURE GetWordInfoById
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


CREATE PROCEDURE GetWordsByUserId
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
        hs.Id_user IS NOT NULL
    ORDER BY
        hs.dDatetime DESC; -- Sắp xếp theo dDatetime giảm dần
END;

EXEC GetWordsByUserId 1 

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