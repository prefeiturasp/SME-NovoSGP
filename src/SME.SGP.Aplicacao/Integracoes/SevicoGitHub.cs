using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        public SevicoGithub(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> RecuperarUltimaVersao()
        {
            
            string usuario = configuration.GetSection("UsuarioGithub").Value ?? throw new NegocioException("Não foi possível localizar o usuário github.");
            string senha = configuration.GetSection("SenhaGithub").Value ?? throw new NegocioException("Não foi possível localizar a senha github.");

            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SGP", "1.0"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{usuario}:{senha}")));

            var resposta = await httpClient.GetAsync("repos/prefeiturasp/SME-NovoSGP/tags");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                var versoes =  JsonConvert.DeserializeObject<IEnumerable<VersaoGitHubRetornoDto>>(json);
                
                if (versoes.Any())
                    return $"Versão: {versoes.FirstOrDefault().Name}";
                else return string.Empty;
                
            }
            return string.Empty;
        }
    }
}
