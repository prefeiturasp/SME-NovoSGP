using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAnualTerritorio.ServicoFake
{
    public class ObterComponenteCurricularPorIdQueryHandlerFakePlanoAnual : IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>
    {
        public async Task<DisciplinaDto> Handle(ObterComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new DisciplinaDto()
            {
                Nome = "Componente Teste 1",
                CodigoComponenteCurricular = 1,
                Id = 1
            });
        }
    }
}
