using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandler : IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery, IEnumerable<UsuarioPossuiAtribuicaoEolDto>>
    {
        private readonly IServicoEol servicoEOL;
        private readonly HttpClient httpClient;
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQueryHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<UsuarioPossuiAtribuicaoEolDto>> Handle(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaPorTurmasEDatasAvaliacaoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<UsuarioPossuiAtribuicaoEolDto>();
            var resposta = await httpClient.PostAsync($"professores/{request.UsuarioLogado.CodigoRf}/disciplina/{request.DisciplinaId.ToString()}/turmas", 
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
