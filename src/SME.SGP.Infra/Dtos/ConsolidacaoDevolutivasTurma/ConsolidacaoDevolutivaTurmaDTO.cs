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

    public class DevolutivaTurmaDTO
    {
        public string TurmaId { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class FiltroDevolutivaTurmaDTO
    {
        public string TurmaId { get; set; }
        public int AnoLetivo { get; set; }

        public FiltroDevolutivaTurmaDTO(string turmaId, int anoLetivo)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
        }
    }
}
