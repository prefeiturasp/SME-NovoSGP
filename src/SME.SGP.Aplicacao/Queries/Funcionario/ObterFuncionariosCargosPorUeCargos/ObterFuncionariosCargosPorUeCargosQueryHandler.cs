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
    internal class ObterFuncionariosCargosPorUeCargosQueryHandler : IRequestHandler<ObterFuncionariosCargosPorUeCargosQuery, IEnumerable<FuncionarioCargoDTO>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterFuncionariosCargosPorUeCargosQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<IEnumerable<FuncionarioCargoDTO>> Handle(ObterFuncionariosCargosPorUeCargosQuery request, CancellationToken cancellationToken)
        {

            var cargos = String.Join("&cargos=", request.cargosIdsDaUe);

            var listaRetorno = new List<FuncionarioCargoDTO>();

            if (request.cargosIdsDaUe.Any(a => a == (int)Cargo.Supervisor))
            {
                var supervisores = repositorioSupervisorEscolaDre.ObtemSupervisoresPorUe(request.UeCodigo);
                if (supervisores.Any())
                {
                    foreach (var supervisorParaAdicionar in supervisores)
                    {
                        listaRetorno.Add(new FuncionarioCargoDTO()
                        {
                            CargoId = Cargo.Supervisor,
                            FuncionarioRF = supervisorParaAdicionar.SupervisorId
                        });
                    }
                }
            }

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($"/api/escolas/{request.UeCodigo}/funcionarios/cargos?cargos={cargos}&dreCodigo={request.DreCodigo}");

                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    var listaRetornoEOL = JsonConvert.DeserializeObject<IEnumerable<FuncionarioCargoDTO>>(json) as List<FuncionarioCargoDTO>;
                    if (listaRetornoEOL.Any())
                        listaRetorno.AddRange(listaRetornoEOL);
                }

            }

            if (!listaRetorno.Any())
                throw new NegocioException("Não foi possível obter funcionários da UE.");
            else return listaRetorno;

        }
    }
}