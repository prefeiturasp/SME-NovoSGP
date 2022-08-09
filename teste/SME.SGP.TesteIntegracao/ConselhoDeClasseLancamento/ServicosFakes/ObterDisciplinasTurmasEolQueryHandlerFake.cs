using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.ServicosFakes
{
    public class ObterDisciplinasTurmasEolQueryHandlerFake : IRequestHandler<ObterDisciplinasTurmasEolQuery, IEnumerable<DisciplinaResposta>>
    {
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasTurmasEolQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaResposta>()
            {
                new DisciplinaResposta()
                {
                    CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Nome = "Português",
                    LancaNota = true
                }
            };
        }
    }
}
