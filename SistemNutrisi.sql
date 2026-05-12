CREATE DATABASE DBSistemNutrisi;
GO
USE DBSistemNutrisi;
GO



INSERT INTO Admin (nama, email, password)
VALUES ('Iza', 'iza@gmail.com', 'iza123');

SELECT * FROM Admin;


CREATE TABLE [User] (
    id_user INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(10) NOT NULL
        CHECK (role IN ('admin', 'user'))
);

CREATE TABLE KategoriMakanan (
    id_kategori INT IDENTITY(1,1) PRIMARY KEY,
    nama_kategori VARCHAR(100) NOT NULL
);

CREATE TABLE Makanan (
    id_makanan INT IDENTITY(1,1) PRIMARY KEY,
    id_kategori INT NOT NULL,
    nama_makanan VARCHAR(150) NOT NULL,

    CONSTRAINT FK_Makanan_Kategori
    FOREIGN KEY (id_kategori)
    REFERENCES KategoriMakanan(id_kategori)

);

CREATE TABLE Nutrisi (
    id_nutrisi INT IDENTITY(1,1) PRIMARY KEY,
    id_makanan INT UNIQUE NOT NULL,
    kalori FLOAT,
    protein FLOAT,
    lemak FLOAT,
    karbohidrat FLOAT,

    CONSTRAINT FK_Nutrisi_Makanan
    FOREIGN KEY (id_makanan)
    REFERENCES Makanan(id_makanan)
    ON DELETE CASCADE
);

CREATE TABLE KonsumsiMakanan (
    id_konsumsi INT IDENTITY(1,1) PRIMARY KEY,
    id_makanan INT NOT NULL,
    id_user INT NOT NULL,
    tanggal DATE NOT NULL,
    jumlah INT NOT NULL,

    CONSTRAINT FK_Konsumsi_Makanan
    FOREIGN KEY (id_makanan)
    REFERENCES Makanan(id_makanan),


    CONSTRAINT FK_Konsumsi_User
    FOREIGN KEY (id_user)
    REFERENCES [User](id_user)

);


INSERT INTO Admin (nama, email, password) VALUES
('Iza', 'iza@gmail.com', 'iza123');

DROP table Admin;
drop DATABASE  DBSistemNutrisi;

INSERT INTO [User] (nama, email, password, role)
VALUES ('Iza', 'iza@gmail.com', '12345', 'admin');


USE DBSistemNutrisi;
GO

-- ========================================
-- STORED PROCEDURES: LOGIN & REGISTRASI
-- ========================================

CREATE OR ALTER PROCEDURE sp_Login
    @email VARCHAR(100),
    @password VARCHAR(255)
AS
BEGIN
    SELECT id_user, nama, role FROM [User]
    WHERE email = @email AND password = @password;
END
GO

CREATE OR ALTER PROCEDURE sp_Registrasi
    @nama VARCHAR(100),
    @email VARCHAR(100),
    @password VARCHAR(255),
    @role VARCHAR(10)
AS
BEGIN
    INSERT INTO [User] (nama, email, password, role)
    VALUES (@nama, @email, @password, @role);
END
GO

-- ========================================
-- STORED PROCEDURES: KATEGORI MAKANAN
-- ========================================

CREATE OR ALTER PROCEDURE sp_GetKategori
    @search VARCHAR(100) = NULL
AS
BEGIN
    IF @search IS NULL OR @search = ''
        SELECT id_kategori, nama_kategori FROM KategoriMakanan;
    ELSE
        SELECT id_kategori, nama_kategori FROM KategoriMakanan
        WHERE nama_kategori LIKE '%' + @search + '%';
END
GO

CREATE OR ALTER PROCEDURE sp_InsertKategori
    @nama_kategori VARCHAR(100)
AS
BEGIN
    INSERT INTO KategoriMakanan (nama_kategori) VALUES (@nama_kategori);
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateKategori
    @id_kategori INT,
    @nama_kategori VARCHAR(100)
AS
BEGIN
    UPDATE KategoriMakanan SET nama_kategori = @nama_kategori
    WHERE id_kategori = @id_kategori;
END
GO

CREATE OR ALTER PROCEDURE sp_DeleteKategori
    @id_kategori INT
AS
BEGIN
    -- Cek apakah kategori masih dipakai di tabel Makanan
    IF EXISTS (SELECT 1 FROM Makanan WHERE id_kategori = @id_kategori)
    BEGIN
        RAISERROR('Data Kategori tidak bisa dihapus karena sedang terpakai di data Makanan.', 16, 1);
        RETURN;
    END

    DELETE FROM KategoriMakanan WHERE id_kategori = @id_kategori;
END
GO

-- ========================================
-- STORED PROCEDURES: MAKANAN
-- ========================================

CREATE OR ALTER PROCEDURE sp_GetMakanan
    @search VARCHAR(150) = NULL
AS
BEGIN
    IF @search IS NULL OR @search = ''
        SELECT m.id_makanan, m.nama_makanan, k.nama_kategori
        FROM Makanan m
        JOIN KategoriMakanan k ON m.id_kategori = k.id_kategori;
    ELSE
        SELECT m.id_makanan, m.nama_makanan, k.nama_kategori
        FROM Makanan m
        JOIN KategoriMakanan k ON m.id_kategori = k.id_kategori
        WHERE m.nama_makanan LIKE '%' + @search + '%';
END
GO

CREATE OR ALTER PROCEDURE sp_GetMakananCount
AS
BEGIN
    SELECT COUNT(*) FROM Makanan;
END
GO

CREATE OR ALTER PROCEDURE sp_GetKategoriList
AS
BEGIN
    SELECT id_kategori, nama_kategori FROM KategoriMakanan;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertMakanan
    @id_kategori INT,
    @nama_makanan VARCHAR(150)
AS
BEGIN
    INSERT INTO Makanan (id_kategori, nama_makanan) VALUES (@id_kategori, @nama_makanan);
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateMakanan
    @id_makanan INT,
    @id_kategori INT,
    @nama_makanan VARCHAR(150)
AS
BEGIN
    UPDATE Makanan SET id_kategori = @id_kategori, nama_makanan = @nama_makanan
    WHERE id_makanan = @id_makanan;
END
GO

CREATE OR ALTER PROCEDURE sp_DeleteMakanan
    @id_makanan INT
AS
BEGIN
    -- Cek apakah makanan masih dipakai di Nutrisi atau KonsumsiMakanan
    IF EXISTS (SELECT 1 FROM Nutrisi WHERE id_makanan = @id_makanan)
       OR EXISTS (SELECT 1 FROM KonsumsiMakanan WHERE id_makanan = @id_makanan)
    BEGIN
        RAISERROR('Data Makanan tidak bisa dihapus karena sedang terpakai di data Nutrisi atau Konsumsi.', 16, 1);
        RETURN;
    END

    DELETE FROM Makanan WHERE id_makanan = @id_makanan;
END
GO

-- ========================================
-- STORED PROCEDURES: NUTRISI
-- ========================================

CREATE OR ALTER PROCEDURE sp_GetNutrisi
    @search VARCHAR(150) = NULL
AS
BEGIN
    IF @search IS NULL OR @search = ''
        SELECT m.id_makanan, m.nama_makanan, n.kalori, n.protein, n.lemak, n.karbohidrat
        FROM Makanan m
        LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan;
    ELSE
        SELECT m.id_makanan, m.nama_makanan, n.kalori, n.protein, n.lemak, n.karbohidrat
        FROM Makanan m
        LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan
        WHERE m.nama_makanan LIKE '%' + @search + '%';
END
GO

CREATE OR ALTER PROCEDURE sp_GetMakananList
AS
BEGIN
    SELECT id_makanan, nama_makanan FROM Makanan;
END
GO

CREATE OR ALTER PROCEDURE sp_InsertNutrisi
    @id_makanan INT,
    @kalori FLOAT,
    @protein FLOAT,
    @lemak FLOAT,
    @karbohidrat FLOAT
AS
BEGIN
    INSERT INTO Nutrisi (id_makanan, kalori, protein, lemak, karbohidrat)
    VALUES (@id_makanan, @kalori, @protein, @lemak, @karbohidrat);
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateNutrisi
    @id_makanan INT,
    @kalori FLOAT,
    @protein FLOAT,
    @lemak FLOAT,
    @karbohidrat FLOAT
AS
BEGIN
    UPDATE Nutrisi SET kalori = @kalori, protein = @protein, lemak = @lemak, karbohidrat = @karbohidrat
    WHERE id_makanan = @id_makanan;
END
GO

CREATE OR ALTER PROCEDURE sp_DeleteNutrisi
    @id_makanan INT
AS
BEGIN
    DELETE FROM Nutrisi WHERE id_makanan = @id_makanan;
END
GO

-- ========================================
-- STORED PROCEDURES: KONSUMSI MAKANAN
-- ========================================

CREATE OR ALTER PROCEDURE sp_InsertKonsumsi
    @id_makanan INT,
    @id_user INT,
    @tanggal DATE,
    @jumlah INT
AS
BEGIN
    INSERT INTO KonsumsiMakanan (id_makanan, id_user, tanggal, jumlah)
    VALUES (@id_makanan, @id_user, @tanggal, @jumlah);
END
GO

-- ========================================
-- STORED PROCEDURES: RIWAYAT KONSUMSI
-- ========================================

CREATE OR ALTER PROCEDURE sp_GetRiwayatKonsumsi
    @id_user INT
AS
BEGIN
    SELECT k.tanggal, m.nama_makanan, k.jumlah,
           (n.kalori * k.jumlah) AS total_kalori,
           (n.protein * k.jumlah) AS total_protein,
           (n.lemak * k.jumlah) AS total_lemak,
           (n.karbohidrat * k.jumlah) AS total_karbohidrat
    FROM KonsumsiMakanan k
    JOIN Makanan m ON k.id_makanan = m.id_makanan
    LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan
    WHERE k.id_user = @id_user
    ORDER BY k.tanggal DESC;
END
GO

-- ========================================
-- STORED PROCEDURES: DATA NUTRISI UNTUK USER
-- ========================================

CREATE OR ALTER PROCEDURE sp_GetNutrisiUser
AS
BEGIN
    SELECT m.nama_makanan, n.kalori, n.protein, n.lemak, n.karbohidrat
    FROM Makanan m
    LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan;
END
GO


CREATE OR ALTER VIEW v_MakananLengkap AS
SELECT m.id_makanan, m.nama_makanan, m.id_kategori, k.nama_kategori
FROM Makanan m
JOIN KategoriMakanan k ON m.id_kategori = k.id_kategori;
GO

CREATE OR ALTER VIEW v_NutrisiLengkap AS
SELECT m.id_makanan, m.nama_makanan, n.kalori, n.protein, n.lemak, n.karbohidrat
FROM Makanan m
LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan;
GO

CREATE OR ALTER VIEW v_RiwayatKonsumsiLengkap AS
SELECT k.id_konsumsi, k.id_user, k.tanggal, m.id_makanan, m.nama_makanan, k.jumlah,
       (ISNULL(n.kalori, 0) * k.jumlah) AS total_kalori,
       (ISNULL(n.protein, 0) * k.jumlah) AS total_protein,
       (ISNULL(n.lemak, 0) * k.jumlah) AS total_lemak,
       (ISNULL(n.karbohidrat, 0) * k.jumlah) AS total_karbohidrat
FROM KonsumsiMakanan k
JOIN Makanan m ON k.id_makanan = m.id_makanan
LEFT JOIN Nutrisi n ON m.id_makanan = n.id_makanan;
GO
