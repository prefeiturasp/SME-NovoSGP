using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodePersistirTurmaDisciplinaQueryHandler : IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public PodePersistirTurmaDisciplinaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<bool> Handle(PodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {

            var dataString = request.Data.ToString("s");
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"professores/{request.CriadoRF}/turmas/{request.TurmaCodigo}/disciplinas/{request.ComponenteParaVerificarAtribuicao}/atribuicao/verificar/data?dataConsulta={dataString}";
            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(json);
                }
                else
                {
                    string erro = $"Não foi possível validar a atribuição do professor no EOL - HttpCode {(int)resposta.StatusCode}";
                    SentrySdk.AddBreadcrumb(erro);
                    throw new NegocioException(erro);
                }
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage($"Erro ao validar a atribuição do professor no EOL - Turma:{request.TurmaCodigo}, Professor:{request.CriadoRF}, Disciplina:{request.ComponenteParaVerificarAtribuicao} - Erro:{e.Message}");
                SentrySdk.CaptureException(e);
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL");
            }
        }
    }
}
