using MediatR;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesIdsDoProfessorNaTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesIdsDoProfessorNaTurmaQuery, long[]>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesIdsDoProfessorNaTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<long[]> Handle(ObterComponentesCurricularesIdsDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"v1/componentes-curriculares/turmas/{request.TurmaCodigo}/funcionarios/{request.ProfessorRF}/perfis/{request.Perfil}/atribuicoes");

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<long[]>(json);
            }
            else return default;
        }
    }
}
