using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.Frequencia.ServicosFakes
{
    public class ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery, IEnumerable<DisciplinaDto>>
    {
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<DisciplinaDto>()
            {
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 1312,
                    CdComponenteCurricularPai = 0,
                    Nome = "Componente 1312",
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = "1"
                }
            });
        }
    }
}
