using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterDisciplinasPorCodigoTurmaQueryHandlerComponente512Fake: IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>
    {
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPorCodigoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<DisciplinaResposta>()
            {
                new DisciplinaResposta()
                {
                    Id = 512,
                    CodigoComponenteCurricular = 512,
                    CodigoComponenteCurricularPai = 0,
                    CodigoComponenteTerritorioSaber = 0,
                    Nome = "REGÊNCIA INFANTIL EMEI 4H",
                    Regencia = true,
                    LancaNota = false,
                    BaseNacional = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 1, Nome = "Base Nacional Comum" },
                }
            });
        }
    }
}