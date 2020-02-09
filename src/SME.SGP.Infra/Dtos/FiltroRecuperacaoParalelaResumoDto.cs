namespace SME.SGP.Infra
{
    public class FiltroRecuperacaoParalelaResumoDto
    {
        //TODO: data anotations
        public int Ano { get; set; }

        public int CicloId { get; set; }
        public long DreId { get; set; }
        public int? NumeroPagina { get; set; }
        public int TurmaId { get; set; }
        public long UeId { get; set; }
    }
}