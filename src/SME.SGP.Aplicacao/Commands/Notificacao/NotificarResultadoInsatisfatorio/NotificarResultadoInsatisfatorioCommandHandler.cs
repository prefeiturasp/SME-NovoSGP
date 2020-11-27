using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarResultadoInsatisfatorioCommandHandler : IRequestHandler<NotificarResultadoInsatisfatorioCommand, bool>
    {
        private readonly IMediator mediator;

        public NotificarResultadoInsatisfatorioCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(NotificarResultadoInsatisfatorioCommand request, CancellationToken cancellationToken)
        {

            var periodoFechamentoBimestres = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, DateTime.Now.AddDays(request.Dias)));

            var percentualReprovacao = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            foreach(var periodoFechamentoBimestre in periodoFechamentoBimestres)
            {
                if(periodoFechamentoBimestre.PeriodoFechamento.UeId != null)
                {
                    var alunosComNotaLancada = await mediator.Send(new ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery(periodoFechamentoBimestre.PeriodoFechamento.UeId.GetValueOrDefault(), periodoFechamentoBimestre.PeriodoEscolarId));

                    if(alunosComNotaLancada != null)
                    {
                        foreach(var turmaId in alunosComNotaLancada.GroupBy(a => a.TurmaId))
                        {
                            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId.Key));
                            foreach (var componenteCurricularId in alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key).GroupBy(a => a.ComponenteCurricularId))
                            {
                                var alunosTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));
                                var alunosComNota = alunosComNotaLancada.Where(a => a.TurmaId == turmaId.Key && a.ComponenteCurricularId == componenteCurricularId.Key);

                                if (alunosComNota.FirstOrDefault().EhConceito)
                                {
                                    VerificaAlunosResultadoInsatisfatorioConceito(alunosComNota, percentualReprovacao, turmaId.Key, componenteCurricularId.Key);
                                }
                                else
                                {
                                    VerificaAlunosResultadoInsatisfatorioNota(alunosComNota, percentualReprovacao, turmaId.Key, componenteCurricularId.Key);
                                }
                            }
                        }
                    }
                }                    
            }
            
            //retorno.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));

            //fechamento nota vai ter uma FK para conceito valores e vai ter um booleano informando sim ou não


            //private async Task EnviarNotasWfAprovacao(long fechamentoTurmaDisciplinaId, PeriodoEscolar periodoEscolar, Usuario usuarioLogado)
            //{
            //    if (notasEnvioWfAprovacao.Any())
            //    {
            //        var lancaNota = !notasEnvioWfAprovacao.First().ConceitoId.HasValue;
            //        var notaConceitoMensagem = lancaNota ? "nota" : "conceito";

            //        var mensagem = await MontaMensagemWfAprovacao(notaConceitoMensagem, periodoEscolar, usuarioLogado);

            //        var wfAprovacaoNota = new WorkflowAprovacaoDto()
            //        {
            //            Ano = DateTime.Today.Year,
            //            NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
            //            EntidadeParaAprovarId = fechamentoTurmaDisciplinaId,
            //            Tipo = WorkflowAprovacaoTipo.AlteracaoNotaFechamento,
            //            TurmaId = turmaFechamento.CodigoTurma,
            //            UeId = turmaFechamento.Ue.CodigoUe,
            //            DreId = turmaFechamento.Ue.Dre.CodigoDre,
            //            NotificacaoTitulo = $"Alteração em {notaConceitoMensagem} final - Turma {turmaFechamento.Nome} ({turmaFechamento.AnoLetivo})",
            //            NotificacaoTipo = NotificacaoTipo.Notas,
            //            NotificacaoMensagem = mensagem
            //        };

            //        wfAprovacaoNota.AdicionarNivel(Cargo.CP);
            //        wfAprovacaoNota.AdicionarNivel(Cargo.Diretor);
            //        wfAprovacaoNota.AdicionarNivel(Cargo.Supervisor);

            //        var idWorkflow = await comandosWorkflowAprovacao.Salvar(wfAprovacaoNota);
            //        foreach (var notaFechamento in notasEnvioWfAprovacao)
            //        {
            //            await repositorioWfAprovacaoNotaFechamento.SalvarAsync(new WfAprovacaoNotaFechamento()
            //            {
            //                WfAprovacaoId = idWorkflow,
            //                FechamentoNotaId = notaFechamento.Id,
            //                Nota = notaFechamento.Nota,
            //                ConceitoId = notaFechamento.ConceitoId
            //            });
            //        }
            //    }
            //}


            return true;
        }

        private void VerificaAlunosResultadoInsatisfatorioConceito(IEnumerable<AlunosFechamentoNotaDto> alunosComNota, double percentualReprovacao, long turmaId, long componenteCurricularId)
        {
            var alunosTurmaComNotaAbaixo = alunosComNota.Where(a => a.EhConceito && a.TurmaId == turmaId && a.ComponenteCurricularId == componenteCurricularId && !a.NotaConceitoAprovado);
            var totalAlunosRI = ((alunosTurmaComNotaAbaixo.Count() / (double)alunosComNota.Count()) * 100);
            if (totalAlunosRI > percentualReprovacao)
            {
                var i = componenteCurricularId;
            }
        }

        private void VerificaAlunosResultadoInsatisfatorioNota(IEnumerable<AlunosFechamentoNotaDto> alunosComNota, double percentualReprovacao, long turmaId, long componenteCurricularId)
        {
            var alunosTurmaComNotaAbaixo = alunosComNota.Where(a => !a.EhConceito && a.TurmaId == turmaId && a.ComponenteCurricularId == componenteCurricularId);
            var totalAlunosRI = ((alunosTurmaComNotaAbaixo.Count() / (double)alunosComNota.Count()) * 100);
            if (totalAlunosRI > percentualReprovacao)
            {
                var i = componenteCurricularId;
            }
        }
    }
}
