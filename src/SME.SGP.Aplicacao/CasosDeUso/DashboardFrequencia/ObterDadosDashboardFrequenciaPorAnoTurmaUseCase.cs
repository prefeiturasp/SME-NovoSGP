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
            if (dadosTotais == null)
                throw new NegocioException("Não foi possível obter os dados de totalização das frequências por período");

            var dreCodigo = "";
            var ueCodigo = "";
            

            if (ueId != -99)
            {
                var ue = await mediator.Send(new ObterUeComDrePorIdQuery(ueId));
                ueCodigo = ue.CodigoUe;
                dreCodigo = ue.Dre.CodigoDre;
            }
            else if (dreId != -99)
                dreCodigo = await mediator.Send(new ObterCodigoDREPorUeIdQuery(dreId));

            var totalEstudantesAgrupado = await ObterQuantidadeAlunosMatriculadosEol(anoLetivo, ueId, modalidade, anoTurma, dreCodigo, ueCodigo);

            return MapearParaDto(dadosFrequenciaAlunos.GroupBy(c => c.DescricaoAnoTurma), dadosTotais.TotalFrequenciaFormatado, totalEstudantesAgrupado);
        }

        private async Task<IEnumerable<IGrouping<string, QuantidadeAlunoMatriculadoDTO>>> ObterQuantidadeAlunosMatriculadosEol(int anoLetivo, long ueId, int modalidade, int anoTurma, string dreCodigo, string ueCodigo)
        {
            var totalAlunos = await mediator.Send(new ObterQuantidadeAlunosEolMatriculadosQuery(anoLetivo, dreCodigo, ueCodigo, modalidade, anoTurma));

            if (totalAlunos == null || !totalAlunos.Any())
                throw new NegocioException("Não foi possível obter a quantidae de alunos do eol");

            IEnumerable<IGrouping<string, QuantidadeAlunoMatriculadoDTO>> totalEstudantesAgrupado;

            if (ueId != -99)
                totalEstudantesAgrupado = totalAlunos.GroupBy(c => c.Turma);
            else
                totalEstudantesAgrupado = totalAlunos.GroupBy(c => c.Ano);

            return totalEstudantesAgrupado;
        }

        private GraficoFrequenciaAlunoDto MapearParaDto(IEnumerable<IGrouping<string, FrequenciaAlunoDashboardDto>> frequenciasAlunosAgrupadas, string tagTotalFrequencia, IEnumerable<IGrouping<string, QuantidadeAlunoMatriculadoDTO>> totalEstudantesAgrupado)
        {
            var dadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();

            foreach (var frequenciasAlunos in frequenciasAlunosAgrupadas)
            {
                var anoTurma = frequenciasAlunos.First().DescricaoAnoTurmaFormatado;
                
                var frequenciaPresente = frequenciasAlunos.FirstOrDefault(f => f.TipoFrequenciaAluno == TipoFrequenciaDashboard.Presentes);
                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Presentes.Name(),
                    TurmaAno = frequenciaPresente != null ? frequenciaPresente.DescricaoAnoTurmaFormatado : anoTurma,
                    Quantidade = frequenciaPresente != null ? frequenciaPresente.Quantidade : 0
                });
                
                var frequenciaRemotos = frequenciasAlunos.FirstOrDefault(f => f.TipoFrequenciaAluno == TipoFrequenciaDashboard.Remotos);
                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Remotos.Name(),
                    TurmaAno = frequenciaRemotos != null ? frequenciaRemotos.DescricaoAnoTurmaFormatado : anoTurma,
                    Quantidade = frequenciaRemotos != null ? frequenciaRemotos.Quantidade : 0
                });
                
                var frequenciaAusentes = frequenciasAlunos.FirstOrDefault(f => f.TipoFrequenciaAluno == TipoFrequenciaDashboard.Ausentes);
                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Ausentes.Name(),
                    TurmaAno = frequenciaAusentes != null ? frequenciaAusentes.DescricaoAnoTurmaFormatado : anoTurma,
                    Quantidade = frequenciaAusentes != null ? frequenciaAusentes.Quantidade : 0
                });
                
                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.TotalEstudantes.Name(),
                    TurmaAno = anoTurma,
                    Quantidade = totalEstudantesAgrupado.First(c => c.Key == frequenciasAlunos.Key).Select(x => x.Quantidade).Sum() 
                });
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
