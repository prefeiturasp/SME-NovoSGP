using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase : AbstractUseCase, IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase
    {
        public ObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime? dataInicio, DateTime? datafim, int? mes, TipoConsolidadoFrequencia tipoConsolidadoFrequencia, bool visaoDre = false)
        {
            var dadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();
            
            var tipoConsolidado = (int)TipoConsolidadoFrequencia.Semanal;
                
            if (tipoConsolidadoFrequencia == TipoConsolidadoFrequencia.Mensal)
            {
                tipoConsolidado = (int)TipoConsolidadoFrequencia.Mensal;

                dataInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, mes.Value, 1);
                datafim = dataInicio.Value.AddMonths(1).AddDays(-1);
            }

            var frequenciaSemanalMensalDtos = await mediator.Send(new ObterFrequenciasConsolidadasPorTurmaMensalSemestralQuery(anoLetivo, dreId, ueId, modalidade, anoTurma, dataInicio.Value, datafim.Value, tipoConsolidado, visaoDre));

            if (frequenciaSemanalMensalDtos == null || !frequenciaSemanalMensalDtos.Any())
                return null;
            
            foreach (var frequenciasGroup in frequenciaSemanalMensalDtos.GroupBy(f => f.Descricao))
            {
                var frequenciaDescricao = frequenciasGroup.FirstOrDefault();
                
                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = DashboardFrequenciaConstants.QuantidadeAbaixoMinimoFrequenciaDescricao,
                    TurmaAno = frequenciaDescricao.Descricao,
                    Quantidade = frequenciaDescricao.QuantidadeAbaixoMinimoFrequencia
                });

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = DashboardFrequenciaConstants.QuantidadeAcimaMinimoFrequenciaDescricao,
                    TurmaAno = frequenciaDescricao.Descricao,
                    Quantidade = frequenciaDescricao.QuantidadeAcimaMinimoFrequencia
                });
            }
            
            var dadosTotal = new TotalFrequenciaEAulasPorPeriodoDto()
            {
                TotalAulas = frequenciaSemanalMensalDtos.FirstOrDefault().TotalAulas,
                TotalFrequencias = frequenciaSemanalMensalDtos.FirstOrDefault().TotalFrequencias
            };
            var totalFrequencia = dadosTotal.TotalFrequenciaFormatado;

            return new GraficoFrequenciaAlunoDto()
            {
                TagTotalFrequencia = totalFrequencia,
                TotalFrequenciaFormatado = totalFrequencia,
                DadosFrequenciaDashboard = dadosFrequenciaDashboard
            };
        }
    }
}
