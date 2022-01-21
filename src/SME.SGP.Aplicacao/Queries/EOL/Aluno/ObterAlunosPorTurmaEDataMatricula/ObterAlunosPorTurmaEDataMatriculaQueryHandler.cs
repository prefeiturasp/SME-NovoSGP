using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEDataMatriculaQueryHandler : IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterAlunosPorTurmaEDataMatriculaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEDataMatriculaQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"turmas/{request.CodigoTurma}/data-matricula-ticks/{request.DataMatricula.Ticks}";
            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<AlunoPorTurmaResposta>>(json);
                }
                else
                {
                    string erro = $"Não foi possível obter os alunos da turma de data aula no EOL - HttpCode {(int)resposta.StatusCode}";
                    throw new NegocioException(erro);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
