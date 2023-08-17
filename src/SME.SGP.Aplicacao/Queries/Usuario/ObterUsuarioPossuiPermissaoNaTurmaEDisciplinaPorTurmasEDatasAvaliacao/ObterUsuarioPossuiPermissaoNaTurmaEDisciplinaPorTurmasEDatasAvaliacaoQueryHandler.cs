using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery, IEnumerable<UsuarioPossuiAtribuicaoEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<UsuarioPossuiAtribuicaoEolDto>> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            var retorno = Enumerable.Empty<UsuarioPossuiAtribuicaoEolDto>();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.PostAsync(string.Format(ServicosEolConstants.URL_PROFESSORES_DISCIPLINA_TURMAS, request.UsuarioLogado.CodigoRf, request.DisciplinaId.ToString()),
                new StringContent(JsonConvert.SerializeObject(request.TurmasIds), Encoding.UTF8, "application/json-patch+json"));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                retorno = JsonConvert.DeserializeObject<List<UsuarioPossuiAtribuicaoEolDto>>(json);
            }
            return retorno;
        }
    }
}
