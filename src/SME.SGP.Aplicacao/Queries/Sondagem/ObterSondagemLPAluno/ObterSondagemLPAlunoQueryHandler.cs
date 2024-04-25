using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSondagemLPAlunoQueryHandler : IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterSondagemLPAlunoQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<SondagemLPAlunoDto> Handle(ObterSondagemLPAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSondagemConstants.Servico);
            var resposta = await httpClient.GetAsync(string.Format(ServicoSondagemConstants.URL_LINGUA_PORTUGUESA_ALUNO, request.TurmaCodigo, request.AlunoCodigo));
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SondagemLPAlunoDto>(json);
            }
            if (!resposta.IsSuccessStatusCode)
                await RegistraLogErro(resposta, $"Código Turma: {request.TurmaCodigo}, Código Aluno: {request.AlunoCodigo}");

            return new SondagemLPAlunoDto();
        }

        private async Task RegistraLogErro(HttpResponseMessage resposta, string parametros)
        {
            var mensagem = await resposta.Content.ReadAsStringAsync();
            var titulo = $"Ocorreu um erro ao obter Sondagem LP Aluno, código de erro: {resposta.StatusCode}";
            await mediator.Send(new SalvarLogViaRabbitCommand(titulo,
                                                              LogNivel.Critico,
                                                              LogContexto.ApiSondagem,
                                                              $"Mensagem: {mensagem}, Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}"));
        }
    }
}
