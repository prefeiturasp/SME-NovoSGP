using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarDadosResponsavelAlunoProdamCommandHandler : IRequestHandler<AtualizarDadosResponsavelAlunoProdamCommand, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public AtualizarDadosResponsavelAlunoProdamCommandHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(AtualizarDadosResponsavelAlunoProdamCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosProdamConstants.SERVICO);
            var url = ServicosProdamConstants.URL_ATUALIZAR_RESPONSAVEL_ALUNO;
            var body = JsonConvert.SerializeObject(request.DadosResponsavelAluno, new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() });
            try
            {
                var resposta = await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

                if (!resposta.IsSuccessStatusCode && resposta.StatusCode == HttpStatusCode.NoContent)
                    throw new NegocioException("Não foi possível atualizar os dados do responsável na prodam.");
            } catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro prodam: {ex.Message}",
                                                    LogNivel.Critico,
                                                    LogContexto.Geral, ex.Message));

                throw new ErroInternoException("Erro prodam.", ex);
            }
            return true;
        }
    }
}
