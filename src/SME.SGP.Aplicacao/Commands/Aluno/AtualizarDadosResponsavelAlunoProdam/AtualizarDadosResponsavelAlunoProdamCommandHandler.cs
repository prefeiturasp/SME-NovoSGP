using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        public AtualizarDadosResponsavelAlunoProdamCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(AtualizarDadosResponsavelAlunoProdamCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosProdamConstants.SERVICO);
            var url = ServicosProdamConstants.URL_ATUALIZAR_RESPONSAVEL_ALUNO;
            var resposta = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request.DadosResponsavelAluno), Encoding.UTF8, "application/json"));

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foi possível atualizar os dados do responsável na prodam.");

            return true;
        }
    }
}
