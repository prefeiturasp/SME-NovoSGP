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
    public class AtualizarDadosResponsavelAlunoEolCommandHandler : IRequestHandler<AtualizarDadosResponsavelAlunoEolCommand, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AtualizarDadosResponsavelAlunoEolCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(AtualizarDadosResponsavelAlunoEolCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_ATUALIZAR_RESPONSAVEIS, request.DadosResponsavelAluno.CodigoAluno, request.DadosResponsavelAluno.Cpf);
            var resposta = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(request.DadosResponsavelAluno), Encoding.UTF8, "application/json"));

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foi possível atualizar os dados do responsável no eol.");

            return true;
        }
    }
}
