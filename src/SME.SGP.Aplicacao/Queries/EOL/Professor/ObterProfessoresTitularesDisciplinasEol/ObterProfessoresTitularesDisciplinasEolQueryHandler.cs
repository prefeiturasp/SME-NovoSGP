using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDisciplinasEolQueryHandler : IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery,IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterProfessoresTitularesDisciplinasEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDisciplinasEolQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            StringBuilder url = new StringBuilder();

            url.Append(string.Format(ServicosEolConstants.URL_PROFESSORES_TITULARES_TURMA, request.CodigoTurma, request.RealizaAgrupamento));

            //Ao passar o RF do professor, o endpoint retorna todas as disciplinas que o professor não é titular para evitar
            //que o professor se atribua como CJ da própria da turma que ele é titular da disciplina
            if (!string.IsNullOrEmpty(request.ProfessorRf))
                url.Append($"?codigoRf={request.ProfessorRf}");

            if (request.DataReferencia.HasValue)
            {
                if (!string.IsNullOrEmpty(request.ProfessorRf))
                    url.Append($"&dataReferencia={request.DataReferencia.Value.ToString("yyyy-MM-dd")}");
                else
                    url.Append($"?dataReferencia={request.DataReferencia.Value.ToString("yyyy-MM-dd")}");
            }

            var resposta = await httpClient.GetAsync(url.ToString());
            if (!resposta.IsSuccessStatusCode)
                return null;

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return null;

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProfessorTitularDisciplinaEol>>(json);
        }
        
    }
}