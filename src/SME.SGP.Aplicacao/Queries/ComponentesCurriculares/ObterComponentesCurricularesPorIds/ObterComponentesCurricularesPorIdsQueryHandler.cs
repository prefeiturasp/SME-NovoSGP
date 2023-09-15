using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, 
                                                              IMediator mediator,
                                                              IHttpClientFactory httpClientFactory)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            var idsComponentesCurricularesAgrupamentoTerritório = request.Ids.Where(id => id >= TerritorioSaberConstants.COMPONENTE_AGRUPAMENTO_TERRITORIO_SABER_ID_INICIAL);
            var idsComponentesCurriculares = request.Ids.Where(id => !idsComponentesCurricularesAgrupamentoTerritório.Contains(id));
            List<DisciplinaDto> retorno = new List<DisciplinaDto>();
            if (idsComponentesCurriculares.Any())
                retorno.AddRange(await repositorioComponenteCurricular.ObterDisciplinasPorIds(idsComponentesCurriculares.ToArray()));
            if (idsComponentesCurricularesAgrupamentoTerritório.Any())
                retorno.AddRange(await ObterDisciplinasAgrupamentoTerritorioPorIds(idsComponentesCurricularesAgrupamentoTerritório.ToArray()));
            return retorno;
        }

        private async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasAgrupamentoTerritorioPorIds(long[] idsComponentesCurricularesAgrupamento)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            var parametros = JsonConvert.SerializeObject(idsComponentesCurricularesAgrupamento);
            
            var resposta = await httpClient.PostAsync(ServicosEolConstants.URL_COMPONENTES_CURRICULARES_AGRUPAMENTO_TERRITORIO_SABER, 
                                                    new StringContent(parametros, Encoding.UTF8, "application/json-patch+json"));
            if (!resposta.IsSuccessStatusCode)
            {
                var mensagem = await resposta.Content.ReadAsStringAsync();
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro ao obter as disciplinas no EOL, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, ", LogNivel.Negocio, LogContexto.ApiEol, string.Empty));
                return Enumerable.Empty<DisciplinaDto>();
            }

            var json = resposta.Content.ReadAsStringAsync().Result;
            var componentesCurricularesEol = JsonConvert.DeserializeObject<List<ComponenteCurricularEol>>(json);
            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(componentesCurricularesEol.ObterCodigos()));
            componentesCurricularesEol.PreencherInformacoesPegagogicasSgp(componentesCurricularesSgp);
            
            return componentesCurricularesEol.Select(cc => new DisciplinaDto()
                    {
                        Id = cc.Codigo,
                        CodigoComponenteCurricular = cc.Codigo,
                        CodigoComponenteCurricularTerritorioSaber = cc.CodigoComponenteTerritorioSaber,
                        CdComponenteCurricularPai = cc.CodigoComponenteCurricularPai,
                        Compartilhada = cc.Compartilhada,
                        GrupoMatrizId = cc.GrupoMatriz?.Id ?? 0,
                        GrupoMatrizNome = cc.GrupoMatriz?.Nome ?? string.Empty,
                        LancaNota = cc.LancaNota,
                        Nome = cc.Descricao,
                        NomeComponenteInfantil = cc.DescricaoComponenteInfantil,
                        Professor = cc.Professor,
                        Regencia = cc.Regencia,
                        RegistraFrequencia = cc.RegistraFrequencia,
                        TerritorioSaber = cc.TerritorioSaber,
                        TurmaCodigo = cc.TurmaCodigo
            });
        }
    }
}
