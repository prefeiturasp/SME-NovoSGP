﻿namespace SME.SGP.Dominio
{
    public class WfAprovacaoItinerancia 
    {
        public long Id { get; set; }
        public long WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long ItineranciaId { get; set; }
        public bool StatusAprovacao { get; set; }
    }
}
