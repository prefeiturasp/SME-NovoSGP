namespace SME.SGP.Infra.Consts
{
    public static class BimestreConstants
    {
        public static string ObterCondicaoBimestre(int bimestre, bool ehModalidadeInfantil)
        {
            return ehModalidadeInfantil ? (bimestre <= 2 ? " <=2 " : " > 2") : " = @bimestre";
        }
    }
}
