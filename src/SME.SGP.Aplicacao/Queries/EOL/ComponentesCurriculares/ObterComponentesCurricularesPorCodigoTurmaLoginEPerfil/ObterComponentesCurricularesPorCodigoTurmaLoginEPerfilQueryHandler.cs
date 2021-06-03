using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQueryHandler : IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"v1/componentes-curriculares/turmas/{request.CodigoTurma}/funcionarios/{request.Login}/perfis/{request.Perfil}";
            try
            {
                var resposta = await httpClient.GetAsync(url);
                if (resposta.IsSuccessStatusCode)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<ComponenteCurricularEol>>(json);
                }
                else
                {
                    string erro = $"Não foi possível validar datas para a atribuição do professor no EOL - HttpCode {(int)resposta.StatusCode}";
                    SentrySdk.AddBreadcrumb(erro);
                    throw new NegocioException(erro);
                }
            }
            catch (Exception e)
            {
                SentrySdk.CaptureMessage($"Erro ao buscar componentes curriculares no EOL - Turma:{request.CodigoTurma}, Login:{request.Login}, Perfil:{request.Perfil} - Erro:{e.Message}");
                SentrySdk.CaptureException(e);
                throw new NegocioException("Não foi possível consultar os componentes curriculares no EOL");
            }
        }
    }
}
