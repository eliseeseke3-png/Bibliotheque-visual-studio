namespace BibliothequeApp.Helpers
{
    /// <summary>
    /// Classe utilitaire pour générer des codes métier uniques
    /// </summary>
    public static class CodeGenerator

    {
      
           
        /// <summary>
        /// Génère un code unique avec un préfixe et 8 chiffres aléatoires
        /// </summary>
        /// <param name="prefix">Préfixe du code (ex: MBR, EMP, ABO, RES)</param>
        /// <returns>Code au format PREFIX + 8 chiffres</returns>
        public static string GenerateCode(string prefix)
        {
            var rand = new Random();
            return prefix + rand.Next(10000000, 99999999).ToString();
        }

        /// <summary>
        /// Génère un code Membre (MBR + 8 chiffres)
        /// </summary>
        public static string GenerateCodeMembre() => GenerateCode("MBR");

        /// <summary>
        /// Génère un code Emprunt (EMP + 8 chiffres)
        /// </summary>
        public static string GenerateCodeEmprunt() => GenerateCode("EMP");

        /// <summary>
        /// Génère un code Abonnement (ABO + 8 chiffres)
        /// </summary>
        public static string GenerateCodeAbonnement() => GenerateCode("ABO");

        /// <summary>
        /// Génère un code Réservation (RES + 8 chiffres)
        /// </summary>
        public static string GenerateCodeReservation() => GenerateCode("RES");
    }
}
