using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommand : IRequest<RelatorioCorrelacaoJasper>
    {
        public ReceberRelatorioProntoCommand(Guid requisicaoId, Guid exportacaoId, Guid correlacaoId, string jSessionId, RelatorioCorrelacao relatorioCorrelacao)
        {
            RequisicaoId = requisicaoId;
            ExportacaoId = exportacaoId;
            CodigoCorrelacao = correlacaoId;
            JSessionId = jSessionId;
            RelatorioCorrelacao = relatorioCorrelacao;
        }

        public Guid RequisicaoId { get; set; }
        public Guid ExportacaoId { get; set; }
        public Guid CodigoCorrelacao { get; set; }
        public string JSessionId { get; }

        public RelatorioCorrelacao RelatorioCorrelacao { get; set; }
    }
}
