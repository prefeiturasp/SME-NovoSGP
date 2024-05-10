using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    public class
    ObterAlunoPorCodigoEolQueryHandler : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunoPorCodigoEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request,CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
            {
                return (await ObterDadosAluno(request.CodigoAluno, request.AnoLetivo,request.ConsideraHistorico, request.FiltrarSituacao, request.VerificarTipoTurma))
                             .OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();
            }

            var alunos = await ObterAluno(request.CodigoAluno,
                                          request.AnoLetivo,
                                          request.ConsideraHistorico,
                                          request.FiltrarSituacao,
                                          request.CodigoTurma,
                                          request.VerificarTipoTurma);

            return alunos ?? await ObterAluno(request.CodigoAluno,
                                          request.AnoLetivo,
                                          !request.ConsideraHistorico,
                                          request.FiltrarSituacao,
                                          request.CodigoTurma,
                                          request.VerificarTipoTurma);
        }


        private async Task<AlunoPorTurmaResposta> ObterAluno(string codigoAluno, int anoLetivo,bool historica, bool filtrarSituacao, string codigoTurma, bool verificarTipoTurma)
        {
            var response = (await ObterDadosAluno(codigoAluno, anoLetivo, historica, filtrarSituacao, verificarTipoTurma))
                                                                        .OrderByDescending(a => a.DataSituacao);

            var retorno = response.Where(da => da.CodigoTurma.ToString().Equals(codigoTurma));

            if (retorno.EhNulo())
                return null;

            return retorno.FirstOrDefault(a => a.EstaAtivo(DateTime.Today.Date));
        }
        
        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codigoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true, bool verificarTipoTurma = true)
        {
            var alunos = Enumerable.Empty<AlunoPorTurmaResposta>();
            
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var url = string.Format(ServicosEolConstants.URL_ALUNOS_TURMAS_ANOS_LETIVOS_HISTORICO_FILTRAR_SITUACAO_TIPO_TURMA, codigoAluno, anoLetivo, consideraHistorico, filtrarSituacao, verificarTipoTurma);

            var resposta = await httpClient.GetAsync(url);
            
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
            }

            return alunos;
        }
    }
}