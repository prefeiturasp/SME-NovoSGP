using MediatR;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProfessorPodePersistirTurmaQueryHandler : IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>
    {
        private readonly IHttpClientFactory httpClient;

        public ProfessorPodePersistirTurmaQueryHandler(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<bool> Handle(ProfessorPodePersistirTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataString = request.DataAula.ToString("s");
            var http = httpClient.CreateClient("servicoEOL");
            var resposta = await http.GetAsync($"professores/{request.ProfessorRf}/turmas/{request.CodigoTurma}/atribuicao/verificar/data?dataConsulta={dataString}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = resposta.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(json);
            }
            else
            {
                throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
            }
        }
    }
}
