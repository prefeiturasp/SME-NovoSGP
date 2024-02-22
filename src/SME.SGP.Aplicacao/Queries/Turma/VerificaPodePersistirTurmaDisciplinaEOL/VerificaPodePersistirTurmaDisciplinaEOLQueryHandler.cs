using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPodePersistirTurmaDisciplinaEOLQueryHandler : IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public VerificaPodePersistirTurmaDisciplinaEOLQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(VerificaPodePersistirTurmaDisciplinaEOLQuery request, CancellationToken cancellationToken)
        {
            var dataString = request.Data.ToString("s");
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var ehTerritorioSaber = request.EhTerritorioSaber;
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_PROFESSORES_TURMAS_DISCIPLINAS_ATRIBUICAO_VERIFICAR_DATA,
                                                                    request.Usuario.CodigoRf,
                                                                    request.TurmaId,
                                                                    request.ComponenteCurricularId) + $"?dataConsulta={dataString}&territorioSaber={ehTerritorioSaber}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
