namespace SME.SGP.Infra.Dtos
{
    public class TurmaParaCopiaPlanoAnualDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool PossuiPlano { get; set; }
        public string TurmaId { get; set; }
    }
}