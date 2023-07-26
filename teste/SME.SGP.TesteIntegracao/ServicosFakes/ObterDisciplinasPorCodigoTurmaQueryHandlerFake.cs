using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterDisciplinasPorCodigoTurmaQueryHandlerFake: IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>
    {
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPorCodigoTurmaQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaResposta>()
            {
                new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = 138,
                    CodigoComponenteCurricularPai = 0,
                    CodigoComponenteTerritorioSaber = 0,
                    Nome = "LINGUA PORTUGUESA",
                    Regencia = false,
                    LancaNota = true,
                    BaseNacional = true,
                    GrupoMatriz = new GrupoMatriz() { Id = 1, Nome = "Base Nacional Comum" },
                }
            };
        }
    }
}