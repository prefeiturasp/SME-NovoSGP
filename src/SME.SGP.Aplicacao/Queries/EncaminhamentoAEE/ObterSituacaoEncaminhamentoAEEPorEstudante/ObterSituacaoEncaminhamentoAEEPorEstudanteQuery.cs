using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorEstudanteQuery : IRequest<SituacaoEncaminhamentoPorEstudanteDto>
    {
        public ObterSituacaoEncaminhamentoAEEPorEstudanteQuery(string estudanteCodigo, string ueCodigo)
        {
            EstudanteCodigo = estudanteCodigo;
            UeCodigo = ueCodigo;
        }

        public string EstudanteCodigo { get; }
        public string UeCodigo { get;  }
    }
}
