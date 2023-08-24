using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQueryHandler: IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery,IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosAtivosTurmaProgramaPapEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunosTurmaProgramaPapDto>> Handle(ObterAlunosAtivosTurmaProgramaPapEolQuery request, CancellationToken cancellationToken)
        {
            if (request.AlunosCodigos.Length > 0 )
            {
                var alunos = string.Join("&codigosAlunos=", request.AlunosCodigos);
                var url = string.Format(ServicosEolConstants.URL_ALUNOS_ALUNOS_PAP, request.AnoLetivo) + $"?codigosAlunos={alunos}";
                var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
                var resposta = await httpClient.GetAsync(url, cancellationToken);
                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
                    return JsonConvert.DeserializeObject<IEnumerable<AlunosTurmaProgramaPapDto>>(json);
                }
            }
            return Enumerable.Empty<AlunosTurmaProgramaPapDto>();
        }
    }
}