using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosSrmPaeeColaborativoEolQueryHandler : IRequestHandler<ObterDadosSrmPaeeColaborativoEolQuery,IEnumerable<DadosSrmPaeeColaborativoEolDto>>
    {
        public ObterDadosSrmPaeeColaborativoEolQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory;
            this.mediator = mediator;
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;
        public async Task<IEnumerable<DadosSrmPaeeColaborativoEolDto>> Handle(ObterDadosSrmPaeeColaborativoEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"srm-paee/aluno/{request.CodigoAluno}";
            
            var resposta = await httpClient.GetAsync(url);
            if(resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DadosSrmPaeeColaborativoEolDto>>(json);
            }
            else 
            {
                var erro = $"Não foi possível obter os dados do SRM/PAEE no EOL - HttpCode {(int)resposta.StatusCode} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));

                throw new NegocioException(erro);
            }
        }
    }
}