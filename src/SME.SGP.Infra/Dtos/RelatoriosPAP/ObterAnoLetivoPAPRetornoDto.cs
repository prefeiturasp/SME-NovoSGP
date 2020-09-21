namespace SME.SGP.Infra
{
    public class ObterAnoLetivoPAPRetornoDto
    {
        public ObterAnoLetivoPAPRetornoDto()
        {
            EhSugestao = false;
        }
        public int Ano { get; set; }
        public bool EhSugestao { get; set; }

    }
}
