﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAcessibilidadesQueryHandler : IRequestHandler<ObterPlanosAEEAcessibilidadesQuery, IEnumerable<AEEAcessibilidadeRetornoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterPlanosAEEAcessibilidadesQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AEEAcessibilidadeRetornoDto>> Handle(ObterPlanosAEEAcessibilidadesQuery request, CancellationToken cancellationToken)
        {
            var query = await repositorio.ObterQuantidadeAcessibilidades(request.Ano, request.DreId, request.UeId);

            if(query.Any())
            {
                List<AEEAcessibilidadeRetornoDto> retorno = new List<AEEAcessibilidadeRetornoDto>();

                var dadosRegular = query.Where(s => s.Descricao.ToUpper() == "REGULAR");
                retorno.Add(new AEEAcessibilidadeRetornoDto()
                {
                    Descricao = "Usa recursos de acessibilidade na sala regular",
                    QuantidadeSim = dadosRegular.FirstOrDefault(a => a.Opcao.ToUpper() == "SIM").NaoEhNulo() ? dadosRegular.First(a => a.Opcao.ToUpper() == "SIM").Quantidade : 0,
                    QuantidadeNao = dadosRegular.FirstOrDefault(a => a.Opcao.ToUpper() == "NÃO").NaoEhNulo() ? dadosRegular.First(a => a.Opcao.ToUpper() == "NÃO").Quantidade : 0,
                    LegendaSim = "Sim",
                    LegendaNao = "Não",
                });

                var dadosSRM = query.Where(s => s.Descricao.ToUpper() == "SRM");
                retorno.Add(new AEEAcessibilidadeRetornoDto()
                {
                    Descricao = "Usa recursos de acessibilidade na SRM",
                    QuantidadeSim = dadosSRM.FirstOrDefault(a => a.Opcao.ToUpper() == "SIM").NaoEhNulo() ? dadosSRM.First(a => a.Opcao.ToUpper() == "SIM").Quantidade : 0,
                    QuantidadeNao = dadosSRM.FirstOrDefault(a => a.Opcao.ToUpper() == "NÃO").NaoEhNulo() ? dadosSRM.First(a => a.Opcao.ToUpper() == "NÃO").Quantidade : 0,
                    LegendaSim = "Sim",
                    LegendaNao = "Não",
                });

                return retorno;
            }

            return null;
        }
    }
}
