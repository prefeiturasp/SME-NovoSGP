using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake
{
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandlerFake : IRequestHandler<
        ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>
    {
        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery request, CancellationToken cancellationToken)
        {
            return new List<AbrangenciaTurmaRetorno>()
            {
                new AbrangenciaTurmaRetorno()
                {
                    Id = 1,
                    Codigo = "1"
                }
            };
        }
    }
}