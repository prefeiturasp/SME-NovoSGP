using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasTurmasEolQueryHandler : IRequestHandler<ObterDisciplinasTurmasEolQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterDisciplinasTurmasEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasTurmasEolQuery request, CancellationToken cancellationToken)
        {
            var turmasCodigo = String.Join("&codigoTurmas=", request.CodigosDeTurmas);
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas?codigoTurmas={turmasCodigo}&adicionarComponentesPlanejamento={request.AdicionarComponentesPlanejamento}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var listaEol = JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
                return TransformarParaDtoDisciplina(listaEol);
            }
            else return default;
        }

        private IEnumerable<DisciplinaResposta> TransformarParaDtoDisciplina(IEnumerable<ComponenteCurricularEol> listaEol)
        {
            foreach (var disciplinaEol in listaEol)
            {
                yield return new DisciplinaResposta()
                {

                    CodigoComponenteCurricular = disciplinaEol.Codigo,
                    CodigoComponenteCurricularPai = disciplinaEol.CodigoComponenteCurricularPai,
                    Nome = disciplinaEol.Descricao,
                    Regencia = disciplinaEol.Regencia,
                    Compartilhada = disciplinaEol.Compartilhada,
                    RegistroFrequencia = disciplinaEol.RegistraFrequencia,
                    LancaNota = disciplinaEol.LancaNota,
                    TurmaCodigo = disciplinaEol.TurmaCodigo
                };
            }
        }
    }
}
