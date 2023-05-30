using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase
    {

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do conselho de clase da turma -> aluno. O id da turma bimestre aluno não foram informados", LogNivel.Critico, LogContexto.ConselhoClasse));
                return false;
            }

            var statusNovo = SituacaoConselhoClasse.NaoIniciado;
            var consolidadoTurmaAluno = await mediator.Send(new ObterConselhoClasseConsolidadoPorTurmaAlunoQuery(filtro.TurmaId, filtro.AlunoCodigo));

            consolidadoTurmaAluno ??= new ConselhoClasseConsolidadoTurmaAluno
            {
                AlunoCodigo = filtro.AlunoCodigo,
                TurmaId = filtro.TurmaId,
                Status = statusNovo
            };

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));
            var turmasCodigos = new string[1] { turma.CodigoTurma };
            var componentesComNotaFechamentoOuConselho = await mediator
                                .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, turmasCodigos, filtro.Bimestre, filtro.AlunoCodigo));

            if (!filtro.Inativo)
            {
                var componentesDoAluno = await mediator
                    .Send(new ObterComponentesParaFechamentoAcompanhamentoCCAlunoQuery(filtro.AlunoCodigo, filtro.Bimestre, filtro.TurmaId));

                if (componentesDoAluno != null && componentesDoAluno.Any())
                {
                    if (!filtro.Bimestre.HasValue || filtro.Bimestre == 0)
                    {
                        var fechamento = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = filtro.TurmaId });
                        var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamento.Id));
                        var conselhoClasseAluno = await mediator.Send(new ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery(conselhoClasse.Id, filtro.AlunoCodigo));
                        consolidadoTurmaAluno.ParecerConclusivoId = conselhoClasseAluno?.ConselhoClasseParecerId;
                    }

                    var turmasItinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

                    if (turma.DeveVerificarRegraRegulares() || (turmasItinerarioEnsinoMedio?.Any(a => a.Id == (int)turma.TipoTurma) ?? false))
                    {
                        var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                        var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();

                        tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));

                        if (turmasItinerarioEnsinoMedio != null)
                            tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));

                        var turmasCodigosComplementares = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, filtro.AlunoCodigo, tiposParaConsulta));

                        if (turmasCodigosComplementares.Any())
                        {
                            var turmasComplementares = turmasCodigosComplementares.Select(s => s).Except(turmasCodigos).ToArray();
                            if (turmasComplementares.Any())
                                turmasCodigos.Concat(turmasComplementares);
                        }
                    }

                    var componentesDaTurmaEol = await mediator
                        .Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigos));

                    //Excessão de disciplina ED. Fisica para modalidade EJA
                    if (turma.EhEJA())
                        componentesDaTurmaEol = componentesDaTurmaEol.Where(a => a.Codigo != "6");

                    var possuiComponentesSemNotaConceito = componentesDaTurmaEol
                        .Where(ct => ct.LancaNota && !ct.TerritorioSaber)
                        .Select(ct => ct.Codigo)
                        .Except(componentesComNotaFechamentoOuConselho.Select(cn => cn.Codigo))
                        .Any();

                    if (possuiComponentesSemNotaConceito)
                        statusNovo = SituacaoConselhoClasse.EmAndamento;
                    else
                        statusNovo = SituacaoConselhoClasse.Concluido;
                }

                if (consolidadoTurmaAluno.ParecerConclusivoId != null)
                    statusNovo = SituacaoConselhoClasse.Concluido;
            }

            consolidadoTurmaAluno.Status = statusNovo;
            consolidadoTurmaAluno.DataAtualizacao = DateTime.Now;

            try
            {
                var consolidadoTurmaAlunoId = await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoCommand(consolidadoTurmaAluno));

                //Quando parecer conclusivo, não altera a nota, atualiza somente o parecerId
                if (!filtro.EhParecer)
                {
                    
                    if (componentesComNotaFechamentoOuConselho != null && componentesComNotaFechamentoOuConselho.Any())
                    {
                        var conselhoClasseNotas = await mediator.Send(new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery(turmasCodigos, filtro.Bimestre ?? 0));
                        var fechamentoNotas = await mediator.Send(new ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreEAlunoCodigoQuery(turmasCodigos, filtro.Bimestre ?? 0, filtro.AlunoCodigo));

                        foreach (var componenteCurricular in componentesComNotaFechamentoOuConselho)
                        {
                            if (!componenteCurricular.LancaNota)
                                continue;

                            var nota = !filtro.ComponenteCurricularId.HasValue || (filtro.ComponenteCurricularId.HasValue && componenteCurricular.Codigo.Equals(filtro.ComponenteCurricularId.Value.ToString())) ? filtro.Nota : null;
                            var conceitoId = !filtro.ComponenteCurricularId.HasValue || (filtro.ComponenteCurricularId.HasValue && componenteCurricular.Codigo.Equals(filtro.ComponenteCurricularId.Value.ToString())) ? filtro.ConceitoId : null;

                            if (componenteCurricular.Regencia)
                            {
                                var componentesRegencia = await mediator.Send(new ObterComponentesRegenciaPorAnoEolQuery(
                                                                    turma.TipoTurno == 4 || turma.TipoTurno == 5 ? turma.AnoTurmaInteiro : 0));

                                foreach (var regencia in componentesRegencia)
                                {
                                    await SalvarConsolidacaoConselhoClasseNota(turma, filtro.Bimestre, regencia.Codigo, long.Parse(componenteCurricular.Codigo),
                                                                               filtro.AlunoCodigo, nota, conceitoId, consolidadoTurmaAlunoId, conselhoClasseNotas, fechamentoNotas);
                                }
                                continue;
                            }
                            await SalvarConsolidacaoConselhoClasseNota(turma, filtro.Bimestre, long.Parse(componenteCurricular.Codigo), 0,
                                                                       filtro.AlunoCodigo, nota, conceitoId, consolidadoTurmaAlunoId, conselhoClasseNotas, fechamentoNotas);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na persistência da consolidação do conselho de classe da turma aluno/nota", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message, "SGP", ex.StackTrace, ex.InnerException?.ToString()));
                return false;
            }
        }

        private async Task<bool> SalvarConsolidacaoConselhoClasseNota(Turma turma, int? bimestre, long componenteCurricularId, long? componenteCurricularRegencia, string alunoCodigo, double? notaFiltro, long? conceitoFiltro, long consolidadoTurmaAlunoId, IEnumerable<NotaConceitoBimestreComponenteDto> notaConceitoBimestreComponenteDto, IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotas)
        {
            double? nota = null;
            double? conceito = null;

            if (notaFiltro == null && conceitoFiltro == null)
            {
                var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdTurmaQuery(turma.Id, bimestre));
                IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotasDiciplina = null;
                IEnumerable<NotaConceitoBimestreComponenteDto> conselhoClasseNotasAluno = null;
                if (fechamentoTurma != null)
                {
                    var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id));
                    if (conselhoClasse != null)
                    {
                        conselhoClasseNotasAluno = notaConceitoBimestreComponenteDto
                            .Where(c => c.AlunoCodigo.Equals(alunoCodigo) && c.ConselhoClasseId == conselhoClasse.Id && c.ComponenteCurricularCodigo == componenteCurricularId);
                    }

                    //-> busca os lançamentos do fechamento somente se não existir conselho
                    if (conselhoClasseNotasAluno == null || !conselhoClasseNotasAluno.Any(x => x.ComponenteCurricularCodigo == componenteCurricularId))
                    {
                        if (componenteCurricularRegencia > 0)
                            fechamentoNotasDiciplina = fechamentoNotas.Where(t => t.ComponenteCurricularId == componenteCurricularRegencia.Value);
                        else
                            fechamentoNotasDiciplina = fechamentoNotas.Where(t => t.ComponenteCurricularId == componenteCurricularId);
                    }
                }

                if (conselhoClasseNotasAluno != null && conselhoClasseNotasAluno.Any(x => x.ComponenteCurricularCodigo == componenteCurricularId))
                {
                    nota = conselhoClasseNotasAluno
                        .FirstOrDefault(x => x.ComponenteCurricularCodigo == componenteCurricularId && ((bimestre.HasValue && x.Bimestre == bimestre.Value) || !x.Bimestre.HasValue))?.Nota;

                    conceito = conselhoClasseNotasAluno
                        .FirstOrDefault(x => x.ComponenteCurricularCodigo == componenteCurricularId && ((bimestre.HasValue && x.Bimestre == bimestre.Value) || !x.Bimestre.HasValue))?.ConceitoId;
                }
                else if (fechamentoNotasDiciplina != null && fechamentoNotasDiciplina.Any())
                {
                    nota = fechamentoNotasDiciplina
                        .FirstOrDefault(x => (bimestre.HasValue && x.Bimestre == bimestre.Value) || !x.Bimestre.HasValue)?.Nota;

                    conceito = fechamentoNotasDiciplina
                        .FirstOrDefault(x => (bimestre.HasValue && x.Bimestre == bimestre.Value) || !x.Bimestre.HasValue)?.ConceitoId;
                }
            }

            var consolidadoNota = await mediator.Send(new ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery(consolidadoTurmaAlunoId, bimestre, componenteCurricularId));

            consolidadoNota ??= new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                Bimestre = bimestre,
            };

            consolidadoNota.ComponenteCurricularId = componenteCurricularId;
            consolidadoNota.Nota = (notaFiltro != null ? notaFiltro : nota);
            consolidadoNota.ConceitoId = (long?)(conceitoFiltro != null ? conceitoFiltro : conceito);

            await mediator.Send(new SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand(consolidadoNota));

            return true;
        }
    }
}
