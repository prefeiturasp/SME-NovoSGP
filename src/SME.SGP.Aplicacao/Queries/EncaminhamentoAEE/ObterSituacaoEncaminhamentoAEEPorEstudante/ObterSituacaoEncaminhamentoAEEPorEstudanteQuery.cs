using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorEstudanteQuery : IRequest<SituacaoEncaminhamentoPorEstudanteDto>
    {
        public ObterSituacaoEncaminhamentoAEEPorEstudanteQuery(string codigoEstudante)
        {
            CodigoEstudante = codigoEstudante;
        }

        public string CodigoEstudante { get; }
    }
}
