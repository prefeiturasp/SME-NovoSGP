using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoTurmaUseCase : AbstractUseCase, IObterDadosDashboardFrequenciaPorAnoTurmaUseCase
    {
        public ObterDadosDashboardFrequenciaPorAnoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int anoTurma, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard)
        {
            var dadosFrequenciaAlunos = await mediator.Send(new ObterDadosDashboardFrequenciaPorAnoTurmaQuery(anoLetivo,
                                                                                                              dreId,
                                                                                                              ueId,
                                                                                                              modalidade,
                                                                                                              semestre,
                                                                                                              anoTurma,
                                                                                                              dataInicio,
                                                                                                              datafim,
                                                                                                              mes,
                                                                                                              tipoPeriodoDashboard));

            if (dadosFrequenciaAlunos == null || !dadosFrequenciaAlunos.Any())
                throw new NegocioException("Não foi possível obter as frequências para o filtro selecionado.");


            var dadosTotais = await mediator.Send(new ObterTotalFrequenciaEAulasPorPeriodoQuery(anoLetivo,
                                                                                                dreId,
                                                                                                ueId,
                                                                                                modalidade,
                                                                                                semestre,
                                                                                                anoTurma,
                                                                                                dataInicio,
                                                                                                datafim,
                                                                                                mes,
                                                                                                tipoPeriodoDashboard));
            if(dadosTotais == null)
                throw new NegocioException("Não foi possível obter os dados de totalização das frequências por período");


            return MapearParaDto(dadosFrequenciaAlunos.GroupBy(c => c.DescricaoAnoTurma), dadosTotais.TotalFrequenciaFormatado);            
        }

        private GraficoFrequenciaAlunoDto MapearParaDto(IEnumerable<IGrouping<string,FrequenciaAlunoDashboardDto>> frequenciasAlunosAgrupadas, string tagTotalFrequencia)
        {            
            var dadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();           

            foreach(var frequenciasAlunos in frequenciasAlunosAgrupadas)
            {

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.TotalEstudantes.Name(),
                    TurmaAno = frequenciasAlunos.First().DescricaoAnoTurmaFormatado,
                    Quantidade = frequenciasAlunos.Select(x => x.Quantidade).Sum()
                });

                foreach (var frequencia in frequenciasAlunos)
                {
                    dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                    {
                        Descricao = frequencia.TipoFrequenciaAluno.Name(),
                        TurmaAno = frequencia.DescricaoAnoTurmaFormatado,
                        Quantidade = frequencia.Quantidade
                    });
                }                
            }

           return new GraficoFrequenciaAlunoDto()
            {
                TagTotalFrequencia = tagTotalFrequencia,
                TotalFrequenciaFormatado = tagTotalFrequencia,
                DadosFrequenciaDashboard = dadosFrequenciaDashboard
            };
        }
    }
}
