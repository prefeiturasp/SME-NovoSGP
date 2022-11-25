using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
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
                    Codigo = "1",
                    Id = 1,
                }
            };
        }
    }
}