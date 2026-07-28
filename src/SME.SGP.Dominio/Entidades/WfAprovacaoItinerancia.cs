using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class WfAprovacaoItinerancia 
    {
        [Key]
        public long Id { get; set; }
        public long WfAprovacaoId { get; set; }
        [Computed]
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long ItineranciaId { get; set; }
        public bool StatusAprovacao { get; set; }
    }
}
