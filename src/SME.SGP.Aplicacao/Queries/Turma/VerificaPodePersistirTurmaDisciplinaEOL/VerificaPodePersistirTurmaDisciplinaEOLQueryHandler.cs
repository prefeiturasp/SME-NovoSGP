using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var resposta = await httpClient.GetAsync($"professores/{request.Usuario.CodigoRf}/turmas/{request.TurmaId}/disciplinas/{request.ComponenteCurricularId}/atribuicao/verificar/data?dataConsulta={dataString}");

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Não foi possível validar a atribuição do professor no EOL.");

            var json = resposta.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
