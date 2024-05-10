using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase : AbstractUseCase, IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase
    {
        public ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase(IMediator mediator,IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoPorTurmaDashBoardFrequencia>();
            
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(filtro.TurmaId));

            var anoLetivo = filtro.AnoLetivo;
            var ehAnosAnterior = anoLetivo < DateTimeExtension.HorarioBrasilia().Year;
            var mes = filtro.Mes;
            var primeiroDiaDoMes = new DateTime(anoLetivo, mes, 1);
            var ultimoDiaDoMes = new DateTime(anoLetivo, mes, DateTime.DaysInMonth(anoLetivo, mes));
            
            var frequenciasParaConsolidar = await mediator.Send(new ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery(anoLetivo,
                filtro.TurmaId,turma.ModalidadeCodigo,filtro.DataAula));
            
            if (frequenciasParaConsolidar.EhNulo())
                return false;
            
            var alunos = await mediator
                .Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma, (primeiroDiaDoMes, ultimoDiaDoMes)));
            
            var frequenciasFinais = 
                (from a in alunos
                    join ft in frequenciasParaConsolidar on a.CodigoAluno equals ft.CodigoAluno
                    where (ehAnosAnterior && !a.Inativo && a.DataMatricula.Date <= ultimoDiaDoMes.Date) ||
                          (!ehAnosAnterior && a.DataMatricula.Date <= ultimoDiaDoMes.Date)
                    select new {a.CodigoAluno, ft.DataAula, ft.TotalAulas, ft.TotalFrequencias, ft.Presentes, ft.Ausentes, ft.Remotos})
                .GroupBy(g=> new {g.DataAula, g.TotalAulas, g.TotalFrequencias})
                .Select(f => new DadosParaConsolidacaoDashBoardFrequenciaDto()
                {
                    DataAula = f.Key.DataAula,
                    TotalAulas = f.Key.TotalAulas,
                    TotalFrequencias = f.Key.TotalFrequencias,
                    Presentes = f.Sum(s => s.Presentes),
                    Ausentes = f.Sum(s => s.Ausentes),
                    Remotos = f.Sum(s => s.Remotos)
                }).ToList();

            foreach (var frequencia in frequenciasFinais)
                await SalvarConsolidacaoDashBoardFrequencia(filtro.AnoLetivo, frequencia, turma, mes);
                
            return true;
        }

        private async Task SalvarConsolidacaoDashBoardFrequencia(int anoLetivo, DadosParaConsolidacaoDashBoardFrequenciaDto frequencia, Turma turma, int mes)
        {
            var consolidacaoDashBoardFrequencia = await mediator.Send(new ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery(turma.Id,
                    frequencia.DataAula, turma.ModalidadeCodigo, anoLetivo, turma.Ue.DreId, turma.Ue.Id, TipoPeriodoDashboardFrequencia.Diario)) ?? new ConsolidacaoDashBoardFrequencia();

            await mediator.Send(new SalvarConsolidacaoDashBoardFrequenciaCommand(MapearParaEntidade(
                consolidacaoDashBoardFrequencia, turma, frequencia, (int)TipoPeriodoDashboardFrequencia.Diario)));
        }

        private ConsolidacaoDashBoardFrequencia MapearParaEntidade(ConsolidacaoDashBoardFrequencia consolidacaoDashBoardFrequencia, Turma turma, DadosParaConsolidacaoDashBoardFrequenciaDto dados, int tipoPeriodo)
        {
            consolidacaoDashBoardFrequencia.AnoLetivo = turma.AnoLetivo;
            consolidacaoDashBoardFrequencia.TurmaId = turma.Id;
            consolidacaoDashBoardFrequencia.TurmaNome = turma.NomeComModalidade();
            consolidacaoDashBoardFrequencia.TurmaAno = turma.AnoComModalidade();
            consolidacaoDashBoardFrequencia.semestre = turma.Semestre;
            consolidacaoDashBoardFrequencia.DataAula = dados.DataAula;
            consolidacaoDashBoardFrequencia.ModalidadeCodigo = (int)turma.ModalidadeCodigo;
            consolidacaoDashBoardFrequencia.Tipo = tipoPeriodo;
            consolidacaoDashBoardFrequencia.DreId = turma.Ue.DreId;
            consolidacaoDashBoardFrequencia.DreCodigo = turma.Ue.Dre.CodigoDre;
            consolidacaoDashBoardFrequencia.UeId = turma.UeId;
            consolidacaoDashBoardFrequencia.DreAbreviacao = AbreviacaoDreFormatado(turma.Ue.Dre.Abreviacao);
            consolidacaoDashBoardFrequencia.QuantidadePresencas = dados.Presentes;
            consolidacaoDashBoardFrequencia.QuantidadeRemotos = dados.Remotos;
            consolidacaoDashBoardFrequencia.QuantidadeAusentes = dados.Ausentes;
            consolidacaoDashBoardFrequencia.CriadoEm = DateTimeExtension.HorarioBrasilia();
            consolidacaoDashBoardFrequencia.TotalAulas = dados.TotalAulas;
            consolidacaoDashBoardFrequencia.TotalFrequencias = dados.TotalFrequencias;
            return consolidacaoDashBoardFrequencia;
        }
        private static string AbreviacaoDreFormatado(string abreviacaoDre)
            => abreviacaoDre.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim();  
    }
}
