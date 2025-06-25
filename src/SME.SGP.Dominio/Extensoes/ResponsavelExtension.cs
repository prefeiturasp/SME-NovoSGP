namespace SME.SGP.Dominio
{
    public static class ResponsavelExtension
    {
        public static string ObterTipoResponsavel(string tipoResponsavel)
        {
            switch (tipoResponsavel)
            {
                case "1": return TipoResponsavel.Filiacao1.ObterNome();
                case "2": return TipoResponsavel.Filiacao2.ObterNome();
                case "3": return TipoResponsavel.ResponsavelLegal.ObterNome();
                case "4": return TipoResponsavel.ProprioEstudante.ObterNome();
                case "5": return TipoResponsavel.ResponsavelEstrangeiroOuNaturalizado.ObterNome();
            }
            return TipoResponsavel.Filiacao1.ToString();
        }
    }
}
