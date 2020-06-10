using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommand : IRequest<bool>
    {
        public Guid RequisicaoId { get; set; }
        public Guid ExportacaoId { get; set; }
        public Guid CorrelacaoId { get; set; }
        public string JSessionId { get; }
    }
}
