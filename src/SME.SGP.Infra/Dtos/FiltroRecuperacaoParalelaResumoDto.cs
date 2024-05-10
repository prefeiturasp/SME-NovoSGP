namespace SME.SGP.Infra
{
    public class FiltroRecuperacaoParalelaResumoDto
    {
        //TODO: data anotations
        public string Ano { get; set; }

        public int? CicloId { get; set; }
        public string DreId { get; set; }
        public int? NumeroPagina { get; set; }
        public int? Periodo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public int AnoLetivo { get; set; }
    }
}