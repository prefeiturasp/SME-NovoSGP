using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterDisciplinasPorCodigoTurmaQueryHandlerFake : IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>
    {
        public const string COMPONENTE_PORTUGUES_DESCRICAO = "Língua Portuguesa";
        public ObterDisciplinasPorCodigoTurmaQueryHandlerFake() { }

        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPorCodigoTurmaQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaResposta>()
            {
                new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = 138,
                    TerritorioSaber = false,
                    Id = 138,
                    RegistroFrequencia = true,
                    LancaNota = true,
                    Nome = COMPONENTE_PORTUGUES_DESCRICAO
                }
            };
        }
    }
}
