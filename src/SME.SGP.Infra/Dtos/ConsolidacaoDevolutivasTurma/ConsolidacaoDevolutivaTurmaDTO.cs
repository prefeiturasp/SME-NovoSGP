namespace SME.SGP.Infra.Dtos
{
    public class ConsolidacaoDevolutivaTurmaDTO
    {
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }
    }
    
    public class QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO
    {
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string TurmaCodigo { get; set; }
        public long TurmaId { get; set; }
        public int QtdeDiarioBordoRegistrados { get; set; }
        public int QtdeRegistradaDevolutivas { get; set; }
    }

    public class DevolutivaTurmaDTO
    {
        public long TurmaId { get; set; }
    }

    public class FiltroDevolutivaTurmaDTO
    {
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }

        public FiltroDevolutivaTurmaDTO(long turmaId, int anoLetivo)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
        }
    }
}
