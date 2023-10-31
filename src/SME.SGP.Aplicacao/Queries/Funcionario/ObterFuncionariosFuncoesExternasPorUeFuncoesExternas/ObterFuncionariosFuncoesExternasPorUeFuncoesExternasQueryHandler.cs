using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQueryHandler : IRequestHandler<ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery, IEnumerable<FuncionarioFuncaoExternaDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<IEnumerable<FuncionarioFuncaoExternaDTO>> Handle(ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery request, CancellationToken cancellationToken)
        {

            var funcoes = string.Join("&funcoes=", request.FuncoesExternasIds);
            var listaRetorno = Enumerable.Empty<FuncionarioFuncaoExternaDTO>();

            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_FUNCIONARIOS_FUNCOES_EXTERNAS, request.UeCodigo, $"?funcoes={funcoes}&dreCodigo={request.DreCodigo}"));

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    listaRetorno = JsonConvert.DeserializeObject<IEnumerable<FuncionarioFuncaoExternaDTO>>(json) as List<FuncionarioFuncaoExternaDTO>;
                }

            }

            if (!listaRetorno.Any())
                throw new NegocioException("Não foi possível obter funcionários externos da UE.");
            else return listaRetorno;

        }
    }
}