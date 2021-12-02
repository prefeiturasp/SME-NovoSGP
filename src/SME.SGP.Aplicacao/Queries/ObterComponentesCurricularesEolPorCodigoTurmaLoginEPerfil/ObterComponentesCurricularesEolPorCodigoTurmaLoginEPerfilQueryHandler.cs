using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandler : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var componenteCurricularEol = new List<ComponenteCurricularEol>();
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas/{request.CodigoTurma}/funcionarios/{request.Login}/perfis/{request.Perfil}/validarVigencia/{request.RealizarAgrupamentoComponente}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                componenteCurricularEol = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            }
            return componenteCurricularEol;
        }
    }
}
