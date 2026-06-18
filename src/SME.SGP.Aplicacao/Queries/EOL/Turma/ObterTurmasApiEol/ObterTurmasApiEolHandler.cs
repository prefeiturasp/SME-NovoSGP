using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasApiEolHandler : IRequestHandler<ObterTurmasApiEolQuery, IList<TurmaApiEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasApiEolHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IList<TurmaApiEolDto>> Handle(ObterTurmasApiEolQuery request, CancellationToken cancellationToken)
        {
            var listaTurmas = new List<TurmaApiEolDto>();
            var filtro = JsonConvert.SerializeObject(request.CodigosTurmas);

            //servicoEOL - ServicosEolConstants.SERVICO
            //using (var httpClient = httpClientFactory.CreateClient("apiEOL"))
            //using (var httpClient = httpClientFactory.CreateClient("UrlApiEOL"))
            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                var resposta = await httpClient.PostAsync("turmas/listar-turmas", new StringContent(filtro, Encoding.UTF8, "application/json-patch+json"));

                if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                    return listaTurmas;

                var json = await resposta.Content.ReadAsStringAsync();

                listaTurmas = JsonConvert.DeserializeObject<List<TurmaApiEolDto>>(json);
            }

            return listaTurmas;
        }
    }
}
