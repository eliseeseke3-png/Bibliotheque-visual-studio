namespace BibliothequeApp.Helpers
{
    public static class AbonnementHelper
    {
        private static readonly Dictionary<string, (decimal Montant, int DureeMois)> Tarifs = new()
        {
            { "Mensuel",      (2000m,  1)  },
            { "Trimestriel",  (5000m,  3)  },
            { "Annuel",       (15000m, 12) }
        };

        public static decimal GetMontant(string? type) =>
            Tarifs.TryGetValue(type ?? "", out var t) ? t.Montant : 0;

        public static int GetDuree(string? type) =>
            Tarifs.TryGetValue(type ?? "", out var t) ? t.DureeMois : 0;

        public static string CalculerStatut(string? dateFin)
        {
            if (DateTime.TryParse(dateFin, out var fin))
                return fin >= DateTime.Today ? "Actif" : "Expiré";
            return "Inconnu";
        }
    }
}