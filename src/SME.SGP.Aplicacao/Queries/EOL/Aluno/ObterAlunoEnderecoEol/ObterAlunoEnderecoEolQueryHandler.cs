using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoEnderecoEolQueryHandler : IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunoEnderecoEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AlunoEnderecoRespostaDto> Handle(ObterAlunoEnderecoEolQuery request, CancellationToken cancellationToken)
        {
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_INFORMACOES, request.CodigoAluno);
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocioEOL.NAO_LOCALIZADO_INFORMACOES_ALUNO_EOL);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AlunoEnderecoRespostaDto>(json);

        }
    }
}