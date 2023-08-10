using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
    internal class ObterFuncionariosPorUeECargoQueryHandler : IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterFuncionariosPorUeECargoQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<IEnumerable<FuncionarioDTO>> Handle(ObterFuncionariosPorUeECargoQuery request, CancellationToken cancellationToken)
        {

            var listaRetorno = Enumerable.Empty<FuncionarioDTO>();

            if (request.CodigoCargo == (int)Cargo.Supervisor)
                return await ObterSupervisoresUE(request.CodigoUE);

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($"/api/escolas/{request.CodigoUE}/funcionarios/cargos/{request.CodigoCargo}");

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    listaRetorno = JsonConvert.DeserializeObject<IEnumerable<FuncionarioDTO>>(json) as List<FuncionarioDTO>;
                }

            }

            return listaRetorno;
        }

        private async Task<IEnumerable<FuncionarioDTO>> ObterSupervisoresUE(string codigoUE)
        {
            var supervisores = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(codigoUE);

            return supervisores.Select(a => new FuncionarioDTO()
            {
                CodigoRF = a.SupervisorId
            });
        }
    }
}