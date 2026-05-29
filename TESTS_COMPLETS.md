# 🧪 Guide de Test Complet - BibliothequeApp

## ✅ Tests Fonctionnels - Checklist

### 1. AUTHENTIFICATION

#### Test 1.1: Connexion Admin
- [ ] Accédez à `http://localhost:5072/Auth/Login`
- [ ] Entrez: Username `admin`, Password `1234`
- [ ] Cliquez sur "Se connecter"
- [ ] ✅ Vous devez être redirigé vers le Dashboard

#### Test 1.2: Dashboard Admin
- [ ] Vérifiez les statistiques affichées (6 cartes)
- [ ] Cliquez sur "Voir tous les livres"
- [ ] ✅ Vous devez voir la liste des 32 livres

#### Test 1.3: Inscription Membre
- [ ] Cliquez sur "S'inscrire" sur la page Login
- [ ] Remplissez le formulaire:
  - Nom: `Dupont`
  - Prénom: `Jean`
  - Email: `jean@example.com`
  - Téléphone: `+229 90123456`
  - Adresse: `Cotonou, Bénin`
  - Username: `jeandupont`
  - Password: `password123`
  - Confirm Password: `password123`
- [ ] Cliquez sur "S'inscrire"
- [ ] ✅ Vous devez être redirigé vers le login

#### Test 1.4: Connexion Membre
- [ ] Connectez-vous avec: Username `jeandupont`, Password `password123`
- [ ] ✅ Vous devez être redirigé vers le Dashboard

#### Test 1.5: Déconnexion
- [ ] Cliquez sur le bouton de déconnexion (coin supérieur)
- [ ] ✅ Vous devez être redirigé vers le Login

---

### 2. GESTION DES LIVRES

#### Test 2.1: Voir la Liste des Livres
- [ ] Connectez-vous (admin ou membre)
- [ ] Cliquez sur "Livres" dans la navbar
- [ ] ✅ Vous devez voir 32 livres

#### Test 2.2: Recherche de Livre
- [ ] Dans la page Livres, entrez "Harry" dans la barre de recherche
- [ ] Cliquez sur "Rechercher"
- [ ] ✅ Vous devez voir le livre "Harry Potter"

#### Test 2.3: Filtrer par Catégorie
- [ ] Sélectionnez une catégorie dans le dropdown (ex: "Fantasy")
- [ ] ✅ Vous devez voir uniquement les livres de cette catégorie

#### Test 2.4: Voir Détails d'un Livre
- [ ] Cliquez sur le titre ou "Détails" d'un livre
- [ ] ✅ Vous devez voir tous les détails du livre
- [ ] Vérifiez: Titre, Auteur, ISBN, Description, Image, Quantité

#### Test 2.5: Créer un Livre (Admin uniquement)
- [ ] Connectez-vous en tant qu'admin
- [ ] Cliquez sur "Ajouter un livre"
- [ ] Remplissez le formulaire:
  - Titre: `Test Livre`
  - Auteur: `Test Auteur`
  - Catégorie: `Fantasy`
  - ISBN: `9999999999`
  - Description: `Un livre de test`
  - Quantité: `5`
  - Disponibilité: `Disponible`
- [ ] Cliquez sur "Ajouter"
- [ ] ✅ Vous devez voir le message "Livre ajouté avec succès!"
- [ ] ✅ Le livre doit être visible dans la liste

#### Test 2.6: Modifier un Livre (Admin uniquement)
- [ ] Trouvez le livre "Test Livre"
- [ ] Cliquez sur "Modifier"
- [ ] Changez le titre en "Test Livre Modifié"
- [ ] Cliquez sur "Mettre à jour"
- [ ] ✅ Vous devez voir le message "Livre modifié avec succès!"

#### Test 2.7: Supprimer un Livre (Admin uniquement)
- [ ] Trouvez le livre "Test Livre Modifié"
- [ ] Cliquez sur "Supprimer"
- [ ] Confirmez la suppression
- [ ] ✅ Vous devez voir le message "Livre supprimé."

#### Test 2.8: Accès Refusé Membre (Non-Admin)
- [ ] Connectez-vous en tant que membre
- [ ] Vérifiez que les boutons "Ajouter", "Modifier", "Supprimer" ne sont pas visibles
- [ ] ✅ Vous devez voir uniquement "Détails"

---

### 3. DASHBOARD

#### Test 3.1: Statistiques Correctes
- [ ] Connectez-vous en admin
- [ ] Allez au Dashboard (Home)
- [ ] Vérifiez les statistiques:
  - [ ] Livres disponibles: doit être > 0
  - [ ] Membres actifs: doit correspondre à la BD
  - [ ] Emprunts actifs: doit être 0 (aucun au démarrage)
  - [ ] Pénalités: doit être 0
  - [ ] Réservations: doit être 0
  - [ ] Abonnements: doit être 0

#### Test 3.2: Navigation Dashboard
- [ ] Cliquez sur chaque carte de statistique
- [ ] ✅ Vous devez être redirigé vers la page correspondante

---

### 4. ERREURS ET EXCEPTIONS

#### Test 4.1: Accès Non Authentifié
- [ ] Déconnectez-vous
- [ ] Essayez d'accéder directement à `/Livres`
- [ ] ✅ Vous devez être redirigé vers le Login

#### Test 4.2: ID Invalide
- [ ] Connectez-vous
- [ ] Accédez directement à `/Livres/Details/99999`
- [ ] ✅ Vous devez voir une erreur 404 ou "Not Found"

#### Test 4.3: Formulaire Invalide
- [ ] Connectez-vous en admin
- [ ] Cliquez sur "Ajouter un livre"
- [ ] Laissez le titre vide
- [ ] Cliquez sur "Ajouter"
- [ ] ✅ Vous devez voir une erreur de validation "Le titre est requis"

---

### 5. SÉCURITÉ

#### Test 5.1: Protection CSRF
- [ ] Connectez-vous en admin
- [ ] Ouvrez l'inspecteur (F12)
- [ ] Cherchez le token CSRF dans les formulaires
- [ ] ✅ Vous devez voir un `__RequestVerificationToken`

#### Test 5.2: Hachage des Mots de Passe
- [ ] Ouvrez SQL Server Management Studio
- [ ] Exécutez: `SELECT Username, Password FROM Utilisateurs`
- [ ] ✅ Les mots de passe doivent être hashés (pas en clair)

#### Test 5.3: Vérification du Rôle
- [ ] Créez 2 utilisateurs: admin et membre
- [ ] Vérifiez que les rôles sont corrects
- [ ] L'admin doit avoir accès aux modifications
- [ ] Le membre doit avoir accès limité

---

## 🔄 Tests de Performance

### Test de Chargement des Livres
```
Temps expected: < 500ms pour charger 32 livres
```

### Test de Recherche
```
Temps expected: < 100ms pour rechercher un livre
```

### Test de Dashboard
```
Temps expected: < 300ms pour charger toutes les statistiques
```

---

## 🌐 Tests Responsive

### Test Mobile (368px width)
- [ ] Ouvrez DevTools (F12)
- [ ] Changez en vue mobile (368x667)
- [ ] Naviguez dans l'application
- [ ] ✅ L'interface doit être lisible

### Test Tablet (768px width)
- [ ] Changez la résolution en 768x1024
- [ ] Vérifiez le layout
- [ ] ✅ Les colonnes doivent s'adapter

### Test Desktop (1920px width)
- [ ] Changez la résolution en 1920x1080
- [ ] ✅ L'interface doit être bien distribuée

---

## 🎨 Tests Visuels

### Test Bootstrap 5
- [ ] Vérifiez les couleurs (primary, success, warning, danger)
- [ ] Vérifiez les icônes Bootstrap Icons
- [ ] Vérifiez l'espacement (margins, paddings)
- [ ] ✅ Tout doit être cohérent et moderne

### Test Formulaires
- [ ] Vérifiez les labels
- [ ] Vérifiez les placeholders
- [ ] Vérifiez la validation
- [ ] ✅ Tous les éléments doivent être visibles

---

## 📊 Tests de Données

### Test Seed de Livres
```bash
SELECT COUNT(*) FROM Livres;
```
✅ Résultat attendu: **32**

### Test Utilisateur Admin
```bash
SELECT * FROM Utilisateurs WHERE Username = 'admin';
```
✅ Résultat attendu: 1 enregistrement avec Role = 'Administrateur'

---

## 🐛 Debugging

### Si erreur de base de données
```bash
dotnet ef database drop --force
dotnet ef database update
```

### Si erreur de compilation
```bash
dotnet clean
dotnet restore
dotnet build
```

### Si l'application ne démarre pas
```bash
# Vérifiez les ports
netstat -ano | findstr :5072

# Regardez l'output window de Visual Studio
# Vérifiez que SQL Server est en cours d'exécution
```

---

## 📋 Résumé des Tests

| Domaine | Tests | Status |
|---------|-------|--------|
| **Authentification** | 5 | ✅ |
| **Livres** | 8 | ✅ |
| **Dashboard** | 2 | ✅ |
| **Erreurs** | 3 | ✅ |
| **Sécurité** | 3 | ✅ |
| **Performance** | 3 | ✅ |
| **Responsive** | 3 | ✅ |
| **Visuels** | 2 | ✅ |
| **Données** | 2 | ✅ |
| **Total** | 31 | ✅ |

---

## ✅ Conditions de Succès

Tous les tests ci-dessus doivent passer pour confirmer que le projet est **100% fonctionnel** ✅

---

**Date**: 28 mai 2026  
**Version**: 1.0  
**Statut**: Prêt pour tests complets
