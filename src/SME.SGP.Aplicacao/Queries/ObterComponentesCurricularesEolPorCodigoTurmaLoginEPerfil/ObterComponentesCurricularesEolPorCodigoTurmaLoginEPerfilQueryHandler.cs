using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandler : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }
                                                                       
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var componenteCurricularEol = new List<ComponenteCurricularEol>();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_TURMAS_FUNCIONARIOS_PERFIS, 
                                                                    request.CodigoTurma, 
                                                                    request.Login, 
                                                                    request.Perfil, 
                                                                    request.RealizarAgrupamentoComponente) + $"?checaMotivoDisponibilizacao={request.ChecaMotivoDisponibilizacao}&consideraTurmaInfantil={request.ConsideraTurmaInfantil}");
            
            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var jsonTest = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                Console.WriteLine(jsonTest);
                var json = await resposta.Content.ReadAsStringAsync();
                componenteCurricularEol = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
                var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(componenteCurricularEol.ObterCodigos()));
                componenteCurricularEol.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
                return componenteCurricularEol;
            }
            return componenteCurricularEol;
        }
    }
}
