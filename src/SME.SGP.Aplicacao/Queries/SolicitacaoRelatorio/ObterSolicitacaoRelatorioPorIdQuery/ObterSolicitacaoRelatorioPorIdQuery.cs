using MediatR;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Aplicacao
{
    public class ObterSolicitacaoRelatorioPorIdQuery : IRequest<SolicitacaoRelatorio>
    {
        public ObterSolicitacaoRelatorioPorIdQuery(long solicitacaoRelatorioId)
        {
            SolicitacaoRelatorioId = solicitacaoRelatorioId;
        }

        public long SolicitacaoRelatorioId { get; set; }
    }

}