using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class PodePersistirTurmaNoPeriodoQueryHandler : IRequestHandler<PodePersistirTurmaNoPeriodoQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PodePersistirTurmaNoPeriodoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(PodePersistirTurmaNoPeriodoQuery request, CancellationToken cancellationToken)
        {
            var dataInicioString = request.DataInicio.ToString("s");
            
            var dataFimString = request.DataFim.ToString("s");
            
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_TURMAS_COMPONENTES_ATRIBUICAO_PERIODO_INICIO_FIM,
                request.ProfessorRf, request.CodigoTurma, request.ComponenteCurricularId, dataInicioString, dataFimString);
            
            var resposta = await httpClient.PostAsync(url, new StringContent(""));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
