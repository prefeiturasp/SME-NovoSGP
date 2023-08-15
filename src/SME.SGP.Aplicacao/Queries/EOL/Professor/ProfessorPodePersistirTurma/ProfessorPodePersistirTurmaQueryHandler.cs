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
        private readonly IHttpClientFactory httpClientFactory;

        public ProfessorPodePersistirTurmaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(ProfessorPodePersistirTurmaQuery request, CancellationToken cancellationToken)
        {
            var dataString = request.DataAula.ToString("s");
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"professores/{request.ProfessorRf}/turmas/{request.CodigoTurma}/atribuicao/verificar/data?dataConsulta={dataString}", cancellationToken);

            if (!resposta.IsSuccessStatusCode)
                throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
            
            var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
