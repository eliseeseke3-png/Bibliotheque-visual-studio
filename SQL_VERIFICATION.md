# 🗄️ Commandes SQL de Vérification - BibliothequeApp

## 📊 Vérifier la Structure de la Base de Données

### Voir toutes les tables
```sql
SELECT name FROM sysobjects WHERE xtype = 'U' ORDER BY name;
```

**Résultat attendu:**
```
Abonnements
Emprunts
Livres
Membres
Notifications
Penalites
Reservations
Utilisateurs
```

---

## 📚 Vérifier les Livres

### Compter les livres
```sql
SELECT COUNT(*) as TotalLivres FROM Livres;
```

**Résultat attendu:** `32`

### Voir tous les livres
```sql
SELECT 
	Id_livre, 
	Titre, 
	Auteur, 
	Categorie, 
	Quantite, 
	QuantiteDisponible, 
	Disponibilite
FROM Livres
ORDER BY Id_livre;
```

### Livres par catégorie
```sql
SELECT 
	Categorie, 
	COUNT(*) as NombreLivres
FROM Livres
GROUP BY Categorie
ORDER BY NombreLivres DESC;
```

---

## 👤 Vérifier l'Admin

### Voir tous les utilisateurs
```sql
SELECT 
	Id, 
	Username, 
	Role, 
	Nom, 
	Prenom, 
	Email
FROM Utilisateurs
ORDER BY Id;
```

**Résultat attendu:**
```
ID    Username    Role              Nom     Prenom
1     admin       Administrateur    Admin   Système
```

### Vérifier le hash du mot de passe admin
```sql
SELECT 
	Username, 
	Password,
	Role
FROM Utilisateurs
WHERE Username = 'admin';
```

**Le mot de passe doit être le hash SHA256 de "1234":**
```
A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=
```

---

## 📋 Vérifier les Structures de Colonnes

### Livres
```sql
EXEC sp_help 'Livres';
```

**Colonnes attendues:**
```
Id_livre             int             (PK, Identity)
Titre               nvarchar(255)    (NOT NULL)
Auteur              nvarchar(255)    (NOT NULL)
ISBN                nvarchar(20)     (NULL)
Categorie           nvarchar(100)    (NULL)
Disponibilite       nvarchar(50)     (NOT NULL, DEFAULT)
Quantite            int              (NOT NULL)
QuantiteDisponible  int              (NOT NULL)
ImageUrl            nvarchar(500)    (NULL)
Description         nvarchar(2000)   (NULL)
```

### Emprunts
```sql
EXEC sp_help 'Emprunts';
```

**Colonnes attendues:**
```
Id_emprunt              int               (PK, Identity)
CodeEmprunt             nvarchar(50)      (NULL)
Id_membre               int               (FK NOT NULL)
Id_livre                int               (FK NOT NULL)
Date_emprunt            datetime2         (NOT NULL)
Date_retour_prevue      datetime2         (NOT NULL)
Date_retour_effective   datetime2         (NULL)
Penalite                decimal(15,2)     (NOT NULL)
Statut                  nvarchar(50)      (NOT NULL)
```

### Abonnements
```sql
EXEC sp_help 'Abonnements';
```

**Colonnes attendues:**
```
Id_abonnement       int               (PK, Identity)
CodeAbonnement      nvarchar(50)      (NULL)
Id_membre           int               (FK NOT NULL)
DateDebut           datetime2         (NOT NULL)
DateFin             datetime2         (NOT NULL)
Actif               bit               (NOT NULL)
Montant             decimal(15,2)     (NOT NULL)
```

---

## 🔗 Vérifier les Clés Étrangères

### Voir toutes les FK
```sql
SELECT 
	OBJECT_NAME(fk.parent_object_id) AS TableFille,
	OBJECT_NAME(fk.referenced_object_id) AS TableParent,
	COL_NAME(fk.parent_object_id, fkc.parent_column_id) AS ColFille,
	COL_NAME(fk.referenced_object_id, fkc.referenced_column_id) AS ColParent
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc 
	ON fk.object_id = fkc.constraint_object_id
ORDER BY TableFille;
```

**Résultat attendu:**
```
Abonnements    → Membres   (Id_membre)
Emprunts       → Membres   (Id_membre)
Emprunts       → Livres     (Id_livre)
Penalites      → Emprunts   (Id_emprunt)
Reservations   → Membres   (Id_membre)
Reservations   → Livres     (Id_livre)
Notifications  → Membres   (Id_membre)
```

---

## 🔍 Vérifier les Index

### Voir tous les index
```sql
SELECT 
	OBJECT_NAME(i.object_id) AS TableName,
	i.name AS IndexName,
	c.name AS ColumnName,
	i.is_unique AS IsUnique,
	i.is_primary_key AS IsPrimaryKey
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE OBJECTPROPERTY(i.object_id, 'IsUserTable') = 1
ORDER BY TableName, IndexName;
```

---

## 📈 Statistiques de Données

### Compter les enregistrements par table
```sql
SELECT 
	'Livres' as Table_Name, COUNT(*) as Count FROM Livres
UNION ALL
SELECT 'Utilisateurs', COUNT(*) FROM Utilisateurs
UNION ALL
SELECT 'Membres', COUNT(*) FROM Membres
UNION ALL
SELECT 'Emprunts', COUNT(*) FROM Emprunts
UNION ALL
SELECT 'Reservations', COUNT(*) FROM Reservations
UNION ALL
SELECT 'Abonnements', COUNT(*) FROM Abonnements
UNION ALL
SELECT 'Penalites', COUNT(*) FROM Penalites
UNION ALL
SELECT 'Notifications', COUNT(*) FROM Notifications
ORDER BY Table_Name;
```

---

## 🧹 Nettoyer la Base de Données (SI NÉCESSAIRE)

### ⚠️ ATTENTION: Ces commandes vont SUPPRIMER les données!

#### Vider tous les emprunts
```sql
DELETE FROM Emprunts;
DBCC CHECKIDENT('Emprunts', RESEED, 0);
```

#### Vider tous les livres et réinitialiser seed
```sql
DELETE FROM Livres;
DBCC CHECKIDENT('Livres', RESEED, 0);
```

#### Supprimer et recréer la base complète
```sql
USE master;
ALTER DATABASE BibliothequeDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE BibliothequeDB;
-- Puis exécuter: dotnet ef database update
```

---

## 🔒 Sécurité

### Voir tous les logins
```sql
SELECT name, type_desc, create_date 
FROM sys.server_principals 
WHERE type IN ('S', 'U')
ORDER BY name;
```

### Voir les permissions par utilisateur
```sql
SELECT 
	dp.name AS DatabaseUser,
	drp.name AS DatabaseRole,
	dp.type_desc AS Type
FROM sys.database_principals dp
LEFT JOIN sys.database_role_members drm ON dp.principal_id = drm.member_principal_id
LEFT JOIN sys.database_principals drp ON drm.role_principal_id = drp.principal_id
ORDER BY dp.name;
```

---

## 📝 Logging et Debugging

### Voir les dernières migrations appliquées
```sql
SELECT 
	MigrationId, 
	ProductVersion, 
	CONVERT(datetime, [__EFMigrationsHistory]) as AppliedDate
FROM __EFMigrationsHistory
ORDER BY MigrationId DESC;
```

**Résultat attendu:**
```
MigrationId                              ProductVersion
20260528210418_Add30Books               8.0.10
20260528205756_InitialCreate            8.0.10
```

---

## 💾 Sauvegarde et Restauration

### Sauvegarde manuelle
```sql
BACKUP DATABASE BibliothequeDB 
TO DISK = 'C:\Backups\BibliothequeDB_backup.bak'
WITH INIT, STATS = 10;
```

### Restauration
```sql
RESTORE DATABASE BibliothequeDB 
FROM DISK = 'C:\Backups\BibliothequeDB_backup.bak'
WITH REPLACE;
```

---

## 🎯 Requêtes Utiles pour Développement

### Voir les 10 derniers emprunts
```sql
SELECT TOP 10 
	e.Id_emprunt, 
	m.Nom, 
	m.Prenom, 
	l.Titre, 
	e.Date_emprunt, 
	e.Date_retour_prevue,
	e.Statut
FROM Emprunts e
JOIN Membres m ON e.Id_membre = m.Id_membre
JOIN Livres l ON e.Id_livre = l.Id_livre
ORDER BY e.Date_emprunt DESC;
```

### Voir les livres les plus empruntés
```sql
SELECT TOP 10 
	l.Titre, 
	l.Auteur,
	COUNT(*) as NombreEmprunts
FROM Emprunts e
JOIN Livres l ON e.Id_livre = l.Id_livre
GROUP BY l.Titre, l.Auteur
ORDER BY NombreEmprunts DESC;
```

### Voir les réservations en attente
```sql
SELECT 
	r.Id_reservation, 
	m.Nom, 
	l.Titre, 
	r.Date_reservation, 
	r.Date_expiration
FROM Reservations r
JOIN Membres m ON r.Id_membre = m.Id_membre
JOIN Livres l ON r.Id_livre = l.Id_livre
WHERE r.Statut = 'En attente'
ORDER BY r.Date_reservation;
```

---

## ✅ Checklist de Vérification

- [ ] 32 livres en base
- [ ] Admin user créé
- [ ] Toutes les tables existent
- [ ] Toutes les FK sont correctes
- [ ] Tous les index sont présents
- [ ] Aucune donnée orpheline
- [ ] Performance acceptable (< 100ms queries)

---

**Dernière mise à jour**: 28 mai 2026  
**Version Base de Données**: 1.0
