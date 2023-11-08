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
    Id_wordtype INT,
    Id_user INT,
    sWord NVARCHAR(255) NOT NULL,
    sExample  NVARCHAR(255),
    sDefinition  NVARCHAR(255),
    FOREIGN KEY (Id_Language) REFERENCES tblLanguage(Id),
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
