using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilPlanejamentoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var url = $"v1/componentes-curriculares/turmas/{request.CodigoTurma}/funcionarios/{request.Login}/perfis/{request.Perfil}/planejamento";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                throw new NegocioException("Ocorreu um erro na tentativa de buscar os componentes no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
        }
    }
}
