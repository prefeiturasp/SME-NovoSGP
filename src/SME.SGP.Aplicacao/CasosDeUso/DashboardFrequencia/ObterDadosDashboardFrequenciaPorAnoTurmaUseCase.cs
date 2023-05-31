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

        public async Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
        {

            var dataAula = dataInicio;
            var dadosFrequenciaAlunos = (await mediator.Send(new ObterDadosDashboardFrequenciaPorAnoTurmaQuery(anoLetivo,
                                                                                                              dreId,
                                                                                                              ueId,
                                                                                                              modalidade,
                                                                                                              semestre,
                                                                                                              anoTurma,
                                                                                                              dataAula,
                                                                                                              dataInicio,
                                                                                                              datafim,
                                                                                                              mes,
                                                                                                              tipoPeriodoDashboard,
                                                                                                              visaoDre))).Where(consolidado => (consolidado.Ausentes + consolidado.Presentes + consolidado.Remotos) > 0);
            if (dadosFrequenciaAlunos == null || !dadosFrequenciaAlunos.Any())
                return null;

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

            if ((!string.IsNullOrEmpty(anoTurma) && anoTurma != "-99") && dadosTotais != null)
                dadosTotais = dadosTotais.Where(a => a.DescricaoAnoTurma == anoTurma).ToList();

            var dadosTotal = new TotalFrequenciaEAulasPorPeriodoDto()
            {
                TotalAulas = dadosTotais.Select(a => a.TotalAulas).Sum(),
                TotalFrequencias = dadosTotais.Select(a => a.TotalFrequencias).Sum(),
            };
            var totalFrequencia = dadosTotal != null ? dadosTotal.TotalFrequenciaFormatado : "";

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

            return MapearParaDto(dadosFrequenciaAlunos, totalFrequencia, modalidade);
        }

        private GraficoFrequenciaAlunoDto MapearParaDto(IEnumerable<FrequenciaAlunoDashboardDto> frequenciasAlunos, string tagTotalFrequencia, int modalidade)
        {
            var dadosFrequenciaDashboard = new List<DadosRetornoFrequenciaAlunoDashboardDto>();

            foreach (var frequenciasGroup in frequenciasAlunos.GroupBy(f => f.Descricao))
            {
                var frequencia = frequenciasGroup.FirstOrDefault();
                var totalPresentes = frequenciasGroup.Select(f => f.Presentes).Sum();
                var totalAusentes = frequenciasGroup.Select(f => f.Ausentes).Sum();
                var totalRemotos = frequenciasGroup.Select(f => f.Remotos).Sum();

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Presentes.Name(),
                    TurmaAno = frequencia.Descricao,
                    Quantidade = totalPresentes
                }); ;

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Remotos.Name(),
                    TurmaAno = frequencia.Descricao,
                    Quantidade = totalRemotos
                });

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = TipoFrequenciaDashboard.Ausentes.Name(),
                    TurmaAno = frequencia.Descricao,
                    Quantidade = totalAusentes
                });

                dadosFrequenciaDashboard.Add(new DadosRetornoFrequenciaAlunoDashboardDto()
                {
                    Descricao = modalidade == (int)Modalidade.EducacaoInfantil ? TipoFrequenciaDashboard.TotalCriancas.Name() : TipoFrequenciaDashboard.TotalEstudantes.Name(),
                    TurmaAno = frequencia.Descricao,
                    Quantidade = totalPresentes + totalAusentes + totalRemotos
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
