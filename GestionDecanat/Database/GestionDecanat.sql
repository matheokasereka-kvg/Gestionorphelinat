IF DB_ID('GestionDecanat') IS NULL
    CREATE DATABASE GestionDecanat;
GO
USE GestionDecanat;
GO

IF OBJECT_ID('Notes','U') IS NOT NULL DROP TABLE Notes;
IF OBJECT_ID('AttributionsCours','U') IS NOT NULL DROP TABLE AttributionsCours;
IF OBJECT_ID('Cours','U') IS NOT NULL DROP TABLE Cours;
IF OBJECT_ID('Inscriptions','U') IS NOT NULL DROP TABLE Inscriptions;
IF OBJECT_ID('Enseignants','U') IS NOT NULL DROP TABLE Enseignants;
IF OBJECT_ID('Etudiants','U') IS NOT NULL DROP TABLE Etudiants;
IF OBJECT_ID('AnneesAcademiques','U') IS NOT NULL DROP TABLE AnneesAcademiques;
IF OBJECT_ID('Promotions','U') IS NOT NULL DROP TABLE Promotions;
IF OBJECT_ID('Facultes','U') IS NOT NULL DROP TABLE Facultes;
IF OBJECT_ID('Utilisateurs','U') IS NOT NULL DROP TABLE Utilisateurs;
GO

CREATE TABLE Facultes (
    idFaculte INT IDENTITY(1,1) PRIMARY KEY,
    nomFaculte NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Promotions (
    idPromotion INT IDENTITY(1,1) PRIMARY KEY,
    nomPromotion NVARCHAR(100) NOT NULL,
    idFaculte INT NOT NULL,
    CONSTRAINT FK_Promotions_Facultes FOREIGN KEY(idFaculte) REFERENCES Facultes(idFaculte)
);

CREATE TABLE AnneesAcademiques (
    idAnnee INT IDENTITY(1,1) PRIMARY KEY,
    libelle NVARCHAR(20) NOT NULL UNIQUE,
    estActive BIT NOT NULL DEFAULT 0
);

CREATE TABLE Etudiants (
    idEtudiant INT IDENTITY(1,1) PRIMARY KEY,
    matricule NVARCHAR(30) NOT NULL UNIQUE,
    nom NVARCHAR(80) NOT NULL,
    postnom NVARCHAR(80) NULL,
    prenom NVARCHAR(80) NULL,
    sexe NVARCHAR(1) NOT NULL CHECK (sexe IN ('M','F')),
    dateNaissance DATE NOT NULL,
    adresse NVARCHAR(200) NULL,
    telephone NVARCHAR(30) NULL
);

CREATE TABLE Inscriptions (
    idInscription INT IDENTITY(1,1) PRIMARY KEY,
    idEtudiant INT NOT NULL,
    idPromotion INT NOT NULL,
    idAnnee INT NOT NULL,
    CONSTRAINT FK_Inscriptions_Etudiants FOREIGN KEY(idEtudiant) REFERENCES Etudiants(idEtudiant),
    CONSTRAINT FK_Inscriptions_Promotions FOREIGN KEY(idPromotion) REFERENCES Promotions(idPromotion),
    CONSTRAINT FK_Inscriptions_Annees FOREIGN KEY(idAnnee) REFERENCES AnneesAcademiques(idAnnee),
    CONSTRAINT UQ_Inscriptions UNIQUE(idEtudiant, idPromotion, idAnnee)
);

CREATE TABLE Enseignants (
    idEnseignant INT IDENTITY(1,1) PRIMARY KEY,
    nom NVARCHAR(80) NOT NULL,
    postnom NVARCHAR(80) NULL,
    prenom NVARCHAR(80) NULL,
    telephone NVARCHAR(30) NULL,
    specialite NVARCHAR(120) NULL
);

CREATE TABLE Cours (
    idCours INT IDENTITY(1,1) PRIMARY KEY,
    nomCours NVARCHAR(120) NOT NULL,
    coefficient DECIMAL(10,2) NOT NULL CHECK (coefficient > 0),
    idFaculte INT NOT NULL,
    idPromotion INT NOT NULL,
    CONSTRAINT FK_Cours_Facultes FOREIGN KEY(idFaculte) REFERENCES Facultes(idFaculte),
    CONSTRAINT FK_Cours_Promotions FOREIGN KEY(idPromotion) REFERENCES Promotions(idPromotion)
);

CREATE TABLE AttributionsCours (
    idAttribution INT IDENTITY(1,1) PRIMARY KEY,
    idCours INT NOT NULL,
    idEnseignant INT NOT NULL,
    CONSTRAINT FK_Attributions_Cours FOREIGN KEY(idCours) REFERENCES Cours(idCours),
    CONSTRAINT FK_Attributions_Enseignants FOREIGN KEY(idEnseignant) REFERENCES Enseignants(idEnseignant),
    CONSTRAINT UQ_Attributions UNIQUE(idCours, idEnseignant)
);

CREATE TABLE Notes (
    idNote INT IDENTITY(1,1) PRIMARY KEY,
    idEtudiant INT NOT NULL,
    idCours INT NOT NULL,
    note DECIMAL(5,2) NOT NULL CHECK (note BETWEEN 0 AND 20),
    dateAjout DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Notes_Etudiants FOREIGN KEY(idEtudiant) REFERENCES Etudiants(idEtudiant),
    CONSTRAINT FK_Notes_Cours FOREIGN KEY(idCours) REFERENCES Cours(idCours),
    CONSTRAINT UQ_Notes UNIQUE(idEtudiant, idCours)
);

CREATE TABLE Utilisateurs (
    idUser INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(30) NOT NULL CHECK(role IN ('Administrateur','Agent Décanat','Enseignant'))
);
GO

INSERT INTO Facultes(nomFaculte) VALUES (N'Sciences Informatiques'), (N'Sciences Économiques');
INSERT INTO Promotions(nomPromotion,idFaculte) VALUES (N'L1 Informatique',1), (N'L2 Informatique',1), (N'L1 Économie',2);
INSERT INTO AnneesAcademiques(libelle,estActive) VALUES (N'2025-2026',1);
INSERT INTO Etudiants(matricule,nom,postnom,prenom,sexe,dateNaissance,adresse,telephone) VALUES
(N'ETU-001',N'KABASELE',N'MBUYI',N'Jean',N'M','2002-04-12',N'Kinshasa',N'0810000001'),
(N'ETU-002',N'ILUNGA',N'KALALA',N'Marie',N'F','2003-08-21',N'Kinshasa',N'0810000002');
INSERT INTO Enseignants(nom,postnom,prenom,telephone,specialite) VALUES (N'MUTOMBO',N'KASONGO',N'Paul',N'0820000001',N'Programmation'), (N'TSHIMANGA',N'KABONGO',N'Anne',N'0820000002',N'Base de données');
INSERT INTO Cours(nomCours,coefficient,idFaculte,idPromotion) VALUES (N'Programmation Graphique',4,1,1), (N'Base de données',3,1,1);
INSERT INTO Inscriptions(idEtudiant,idPromotion,idAnnee) VALUES (1,1,1),(2,1,1);
INSERT INTO AttributionsCours(idCours,idEnseignant) VALUES (1,1),(2,2);
INSERT INTO Notes(idEtudiant,idCours,note) VALUES (1,1,15),(1,2,13),(2,1,9),(2,2,11);
INSERT INTO Utilisateurs(username,password,role) VALUES
(N'admin',N'admin123',N'Administrateur'),
(N'agent',N'agent123',N'Agent Décanat'),
(N'enseignant',N'enseignant123',N'Enseignant');
GO
