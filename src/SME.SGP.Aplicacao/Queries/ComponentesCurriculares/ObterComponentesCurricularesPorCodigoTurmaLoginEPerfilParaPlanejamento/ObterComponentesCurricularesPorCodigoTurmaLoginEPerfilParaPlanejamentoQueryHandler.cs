using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var url = $"v1/componentes-curriculares/turmas/{request.CodigoTurma}/funcionarios/{request.Login}/perfis/{request.Perfil}/planejamento";
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                throw new NegocioException("Ocorreu um erro na tentativa de buscar os componentes no EOL.");
            }

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
        }
    }
}
