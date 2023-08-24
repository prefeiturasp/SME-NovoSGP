using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNecessidadesEspeciaisAlunoEolQueryHandler : IRequestHandler<ObterNecessidadesEspeciaisAlunoEolQuery, InformacoesEscolaresAlunoDto>
    {
        IHttpClientFactory httpClientFactory;

        public ObterNecessidadesEspeciaisAlunoEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<InformacoesEscolaresAlunoDto> Handle(ObterNecessidadesEspeciaisAlunoEolQuery request, CancellationToken cancellationToken)
        {
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_NECESSIDADES_ESPECIAIS, request.CodigoAluno);
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(url);

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.NAO_FORAM_ENCONTRADOS_DADOS_DE_NECESSIDADE_ESPECIAL);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<InformacoesEscolaresAlunoDto>(json);
        }
    }
}
