using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class SevicoGithub : IServicoGithub
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public class Version
        {
            public string Name { get; set; }
        }

        public SevicoGithub(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> RecuperarUltimaVersao()
        {
            String url = configuration.GetSection("UrlApiGithub").Value;
            string usuario = configuration.GetSection("UsuarioGithub").Value;
            string senha = configuration.GetSection("SenhaGithub").Value;

            // TODO: Validar valores vindo da config

            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SGP", "1.0"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{usuario}:{senha}")));

            var resposta = await httpClient.GetAsync(url);
            var content = await resposta.Content.ReadAsStringAsync();
            var versoes = JsonConvert.DeserializeObject<List<Version>>(content);
            return $"Versão: {versoes.FirstOrDefault().Name}";
        }
    }
}
