using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
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
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_PROFESSORES_TURMAS_ATRIBUICAO_VERIFICAR_DATA, request.ProfessorRf, request.CodigoTurma) + $"?dataConsulta={dataString}", cancellationToken);

            if (!resposta.IsSuccessStatusCode)
                throw new Exception("Não foi possível validar a atribuição do professor no EOL.");
            
            var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
