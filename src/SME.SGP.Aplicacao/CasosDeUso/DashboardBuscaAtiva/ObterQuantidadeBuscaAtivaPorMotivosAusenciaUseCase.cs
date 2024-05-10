using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase : IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase
    {
        private readonly IRepositorioDashBoardBuscaAtiva repositorio;
        private readonly IMediator mediator;

        public ObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase(IRepositorioDashBoardBuscaAtiva repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoBuscaAtivaDto> Executar(FiltroGraficoBuscaAtivaDto param)
        {
            var graficos = new GraficoBuscaAtivaDto();
            var consultaDados = await repositorio.ObterDadosGraficoMotivoAusencia(param.AnoLetivo, 
                                                                                  param.Modalidade, 
                                                                                  param.UeId, 
                                                                                  param.DreId,
                                                                                  param.Semestre);

            foreach (var grafico in consultaDados)
            {
                var item = new GraficoBaseDto
                {
                    Quantidade = grafico.Quantidade,
                    Descricao = grafico.MotivoAusencia
                };
                graficos.Graficos.Add(item);
            }

            return graficos;
        }
    }
}