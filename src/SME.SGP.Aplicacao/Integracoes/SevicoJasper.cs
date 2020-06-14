using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class SevicoJasper : ISevicoJasper
    {
        private readonly HttpClient httpClient;
        private readonly JasperCookieHandler jasperCookieHandler;

        public SevicoJasper(HttpClient httpClient, JasperCookieHandler jasperCookieHandler)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.jasperCookieHandler = jasperCookieHandler ?? throw new ArgumentNullException(nameof(jasperCookieHandler));
        }

        public async Task<byte[]> DownloadRelatorio(Guid exportID, Guid requestId, string jSessionId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(jSessionId))
                    jasperCookieHandler.CookieContainer.Add(httpClient.BaseAddress, new System.Net.Cookie("JSESSIONID", jSessionId));

                var resposta = await httpClient.GetAsync($"/jasperserver/rest_v2/reportExecutions/{requestId}/exports/{exportID}/outputResource");
                if (resposta.IsSuccessStatusCode)
                    return await resposta.Content.ReadAsByteArrayAsync();

                return null;


            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
    }
}
