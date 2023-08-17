using Elastic.Apm.Api;
using MediatR;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Ocsp;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;


        public ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesEOLComSemAgrupamentoTurmaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var parametros = JsonConvert.SerializeObject(request.Ids);
            var url = string.Empty;

            if (!request.SemAgrupamentoTurma)
            {
                url = @"disciplinas/turma/";
                if (request.CodigoTurma != null)
                    url += $"?codigoTurma={request.CodigoTurma}";
            }
            else
                url = "disciplinas/SemAgrupamento";

            var resposta = await httpClient.PostAsync(url, new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
            {
                if (resposta.StatusCode != HttpStatusCode.NotFound)
                {
                    var mensagem = await resposta.Content.ReadAsStringAsync();
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter as disciplinas no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Negocio, LogContexto.ApiEol, string.Empty));
                }
                return Enumerable.Empty<DisciplinaDto>();
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = (JsonConvert.DeserializeObject<IEnumerable<RetornoDisciplinaDto>>(json)).MapearDto();

            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(retorno.ObterCodigos()));
            retorno.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
            return retorno;
        }
    }
}
