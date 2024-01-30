namespace SME.SGP.Infra.Consts
{
    public static class BimestreConstants
    {
        public static string ObterCondicaoBimestre(int bimestre, bool ehModalidadeInfantil)
        {
            var condicao = bimestre <= 2 ? " <=2 " : " > 2";

            return ehModalidadeInfantil ? condicao : " = @bimestre";
        }
    }
}
