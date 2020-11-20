namespace SME.SGP.Infra.Dtos
{
    public class PeriodoEscolarPorTurmaDto
    {
        public int Bimestre { get; set; }
        public long Id { get; set; }
        public bool Migrado { get; set; }
        public bool PeriodoAberto { get; set; }
    }
}
