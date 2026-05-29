# 🚀 Guide de Démarrage Rapide - BibliothequeApp

## ⚡ Démarrage

```bash
cd "C:\Users\Elisée\source\repos\Bibliotheque visual studio\Bibliotheque"
dotnet run
```

**L'application démarrera sur**: `http://localhost:5072`

---

## 👤 Comptes de Test

### Admin (Accès Complet)
```
Username: admin
Password: 1234
Rôle: Administrateur
```

**Accès Admin**:
- ✅ Ajouter/Modifier/Supprimer des livres
- ✅ Gérer les membres
- ✅ Gérer les emprunts
- ✅ Gérer les réservations
- ✅ Gérer les abonnements
- ✅ Voir toutes les pénalités

### Créer un compte Membre
1. Cliquez sur "S'inscrire" 
2. Remplissez le formulaire avec:
   - Nom
   - Prénom
   - Email
   - Téléphone (optionnel)
   - Adresse (optionnel)
   - Nom d'utilisateur unique
   - Mot de passe (min 4 caractères)

**Accès Membre**:
- ✅ Voir tous les livres (32 disponibles)
- ✅ Rechercher un livre
- ✅ Filtrer par catégorie
- ✅ Emprunter des livres
- ✅ Réserver des livres
- ✅ Voir ses emprunts actifs

---

## 📚 Livres Disponibles (32 titres)

| Titre | Auteur | Catégorie |
|-------|--------|-----------|
| Harry Potter | J.K. Rowling | Fantasy |
| Le Petit Prince | Antoine de Saint-Exupéry | Conte |
| L'Alchimiste | Paulo Coelho | Développement |
| Père Riche Père Pauvre | Robert T. Kiyosaki | Développement |
| One Piece | Eiichiro Oda | Manga |
| Naruto | Masashi Kishimoto | Manga |
| Dragon Ball | Akira Toriyama | Manga |
| 1984 | George Orwell | Science-fiction |
| Le Seigneur des Anneaux | J.R.R. Tolkien | Fantasy |
| Le Hobbit | J.R.R. Tolkien | Fantasy |
| Sapiens | Yuval Noah Harari | Histoire |
| ...et 21 autres titres! | ... | ... |

---

## 🎯 Fonctionnalités

### Dashboard
- 📊 Statistiques en temps réel
- 📈 Nombre de livres disponibles
- 👥 Total des membres
- 📋 Emprunts actifs
- ⚠️ Pénalités non payées
- 📅 Réservations en attente
- 💳 Abonnements actifs

### Gestion des Livres
- 🔍 Recherche par titre/auteur
- 📂 Filtrage par catégorie
- 📖 Détails complets du livre
- 📸 Images des couvertures
- 💾 Quantité disponible

### Emprunts
- 📤 Emprunter un livre
- 📥 Retourner un livre
- 📅 Délai de 14 jours par défaut
- ⚠️ Pénalité si retard
- 📜 Historique des emprunts

### Réservations
- ✋ Réserver un livre indisponible
- 📭 Notification quand disponible
- 📅 Date d'expiration de la réservation

### Abonnements
- 💳 Types d'abonnement (Basique, Premium)
- 🎁 Avantages selon le type
- 📊 Suivi du statut

---

## 🔐 Sécurité

- ✅ Mots de passe hashés SHA256
- ✅ Sessions sécurisées
- ✅ Rôles d'autorisation
- ✅ Validation des entrées
- ✅ Protection CSRF

---

## 📊 Architecture

```
BibliothequeApp/
├── Controllers/
│   ├── HomeController      (Dashboard)
│   ├── AuthController      (Login/Register)
│   ├── LivresController    (Gestion livres)
│   ├── EmpruntsController  (Gestion emprunts)
│   ├── ReservationsController (Gestion réservations)
│   ├── AbonnementsController (Gestion abonnements)
│   └── ...
├── Models/
│   ├── Livre
│   ├── Emprunt
│   ├── Reservation
│   ├── Abonnement
│   ├── Utilisateur
│   └── ...
├── Views/
│   ├── Home/Index
│   ├── Auth/Login
│   ├── Auth/Register
│   ├── Livres/
│   ├── Emprunts/
│   └── ...
├── Data/
│   └── ApplicationDbContext
├── Migrations/
│   └── InitialCreate
└── Program.cs
```

---

## 🔧 Base de Données

- **Type**: SQL Server (LocalDB)
- **Nom**: BibliothequeDB
- **ORM**: Entity Framework Core 8
- **Tables**: 8 (Livres, Membres, Utilisateurs, Emprunts, Réservations, Abonnements, Pénalités, Notifications)

---

## 💡 Astuces

1. **Connexion rapide**: Username: `admin` / Password: `1234`
2. **Tous les livres ont des images** depuis Open Library API
3. **32 livres de seed** au démarrage automatique
4. **Dashboard responsive** sur mobile et desktop
5. **Validation Bootstrap 5** sur tous les formulaires

---

## ❓ FAQ

**Q: Pourquoi je ne peux pas modifier un livre?**  
R: Seul l'administrateur peut modifier les livres. Connectez-vous avec le compte admin.

**Q: Combien de temps pour emprunter un livre?**  
R: 14 jours par défaut. Passé ce délai, une pénalité s'accumule.

**Q: Les pénalités se payent comment?**  
R: Via le dashboard - vous recevrez une notification si vous avez des retards.

**Q: Puis-je réserver un livre emprunté?**  
R: Oui! Si un livre n'est pas disponible, vous pouvez le réserver. Vous recevrez une notification quand il sera disponible.

---

## 📞 Support

- Vérifiez que SQL Server LocalDB est installé
- Vérifiez que le port 5072 est disponible
- Consultez les logs dans l'Output window de Visual Studio

---

**Dernière mise à jour**: 28 mai 2026  
**Version**: 1.0 - Stable ✅
