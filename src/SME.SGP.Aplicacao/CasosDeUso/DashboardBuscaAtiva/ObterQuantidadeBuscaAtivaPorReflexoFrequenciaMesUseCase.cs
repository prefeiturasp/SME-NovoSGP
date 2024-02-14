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
    public class ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase : IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase
    {
        private readonly IRepositorioDashBoardBuscaAtiva repositorio;
        private readonly IMediator mediator;

        public ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase(IRepositorioDashBoardBuscaAtiva repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GraficoBuscaAtivaDto> Executar(FiltroGraficoReflexoFrequenciaBuscaAtivaDto param)
        {
            var graficos = new GraficoBuscaAtivaDto();
            var consultaDados = await repositorio.ObterDadosGraficoReflexoFrequencia(param.Mes,
                                                                                  param.AnoLetivo, 
                                                                                  param.Modalidade, 
                                                                                  param.UeId, 
                                                                                  param.DreId,
                                                                                  param.Semestre);
            graficos.DataUltimaConsolidacao = await repositorio.ObterDataUltimaConsolidacaoReflexoFrequencia();

            foreach (var grafico in consultaDados)
            {
                var item = new GraficoBaseDto
                {
                    Quantidade = grafico.Quantidade,
                    Descricao = grafico.ReflexoFrequencia,
                    Grupo = param.UeId.HasValue ? $"{grafico.Modalidade.ObterNomeCurto()}-{grafico.Turma}"
                                                : $"{grafico.Ano}º ano"
                };
                graficos.Graficos.Add(item);
            }

            return graficos;
        }
    }
}