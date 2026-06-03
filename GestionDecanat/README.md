# Système de Gestion Académique de Décanat

Application de bureau C# WinForms (.NET Framework 4.8) avec SQL Server, ADO.NET, architecture POO, formulaires CRUD, calculs académiques, statistiques et rapports PDF QuestPDF.

## Installation

1. Ouvrir SQL Server Management Studio et exécuter `Database/GestionDecanat.sql`.
2. Ouvrir `GestionDecanat.sln` avec Visual Studio 2022.
3. Adapter la chaîne de connexion dans `App.config` si votre instance SQL Server n'est pas `.`.
4. Restaurer les packages NuGet puis lancer le projet.

## Comptes de test

- Administrateur : `admin` / `admin123`
- Agent Décanat : `agent` / `agent123`
- Enseignant : `enseignant` / `enseignant123`

## Structure

- `Models` : classes métier POO.
- `DAL` : accès SQL Server avec `SqlConnection`, `SqlCommand`, `SqlDataAdapter` et `DataTable`.
- `Services` : authentification, calculs, statistiques et rapports PDF.
- `Forms` : formulaires WinForms.
- `Database` : script SQL complet.
