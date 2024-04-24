using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSondagemLPAlunoQueryHandler : IRequestHandler<ObterSondagemLPAlunoQuery, SondagemLPAlunoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterSondagemLPAlunoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<SondagemLPAlunoDto> Handle(ObterSondagemLPAlunoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicoSondagemConstants.Servico);
            var resposta = await httpClient.GetAsync(string.Format(ServicoSondagemConstants.URL_LINGUA_PORTUGUESA_ALUNO, request.TurmaCodigo, request.AlunoCodigo));
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SondagemLPAlunoDto>(json);
            }
            return new SondagemLPAlunoDto();
        }
    }
}
