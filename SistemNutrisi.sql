CREATE DATABASE DBSistemNutrisi;
GO
USE DBSistemNutrisi;
GO

CREATE TABLE Admin (
    id_admin INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE [User] (
    id_user INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
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
    ON DELETE CASCADE
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
    REFERENCES Makanan(id_makanan)
    ON DELETE CASCADE,

    CONSTRAINT FK_Konsumsi_User
    FOREIGN KEY (id_user)
    REFERENCES [User](id_user)
    ON DELETE CASCADE
);


INSERT INTO Admin (nama, email, password) VALUES
('Iza', 'iza@gmail.com', 'iza123');