using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommand : IRequest<bool>
    {
        public ReceberRelatorioProntoCommand(Guid requisicaoId, Guid exportacaoId, Guid correlacaoId, string jSessionId)
        {
            RequisicaoId = requisicaoId;
            ExportacaoId = exportacaoId;
            CodigoCorrelacao = correlacaoId;
            JSessionId = jSessionId;
        }

        public Guid RequisicaoId { get; set; }
        public Guid ExportacaoId { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string JSessionId { get; }
    }
}
