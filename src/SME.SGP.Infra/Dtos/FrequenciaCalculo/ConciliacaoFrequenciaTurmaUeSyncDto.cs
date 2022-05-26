namespace SME.SGP.Infra.Dtos
{
    public class ConciliacaoFrequenciaTurmaUeSyncDto
    {
        public ConciliacaoFrequenciaTurmaUeSyncDto(string ueCodigo, int anoLetivo)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
        }

        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }
}
