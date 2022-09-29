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
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota;

        public ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado, IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
            this.repositorioConselhoClasseConsolidadoNota = repositorioConselhoClasseConsolidadoNota ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoNota));
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
            var consolidadoTurmaAluno = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(filtro.TurmaId, filtro.AlunoCodigo);

            consolidadoTurmaAluno ??= new ConselhoClasseConsolidadoTurmaAluno
            {
                AlunoCodigo = filtro.AlunoCodigo,
                TurmaId = filtro.TurmaId,
                Status = statusNovo
            };

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(filtro.TurmaId));

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

                    var turmasCodigos = new string[] { };
                    var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());

                    if (turma.DeveVerificarRegraRegulares() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                    {
                        var turmasCodigosParaConsulta = new List<int>() { (int)turma.TipoTurma };
                        turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                        turmasCodigosParaConsulta.AddRange(turmasitinerarioEnsinoMedio.Select(s => s.Id));
                        turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, filtro.AlunoCodigo, turmasCodigosParaConsulta));
                    }

                    if (turmasCodigos.Length == 0)
                        turmasCodigos = new string[1] { turma.CodigoTurma };

                    var componentesComNotaFechamentoOuConselho = await mediator
                        .Send(new ObterComponentesComNotaDeFechamentoOuConselhoQuery(turma.AnoLetivo, filtro.TurmaId, filtro.Bimestre, filtro.AlunoCodigo));

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
                consolidadoTurmaAluno.Id = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaAlunoAsync(filtro.TurmaId, filtro.AlunoCodigo);

                var consolidadoTurmaAlunoId = await repositorioConselhoClasseConsolidado.SalvarAsync(consolidadoTurmaAluno);

                var consolidadoNota = await repositorioConselhoClasseConsolidadoNota.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(consolidadoTurmaAlunoId, filtro.Bimestre, filtro.ComponenteCurricularId);
                consolidadoNota ??= new ConselhoClasseConsolidadoTurmaAlunoNota()
                {
                    ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                    Bimestre = filtro.Bimestre,
                    ComponenteCurricularId = filtro.ComponenteCurricularId
                };

                //Quando parecer conclusivo, não altera a nota, atualiza somente o parecerId
                if (!filtro.EhParecer)
                {
                    var componentes = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(new string[] { turma.CodigoTurma }, false));

                    if (componentes != null && componentes.Any())
                    {
                        foreach (var componenteCurricular in componentes)
                        {
                            var nota = !filtro.ComponenteCurricularId.HasValue || (filtro.ComponenteCurricularId.HasValue && componenteCurricular.Codigo.Equals(filtro.ComponenteCurricularId.Value)) ? filtro.Nota : null;
                            var conceitoId = !filtro.ComponenteCurricularId.HasValue || (filtro.ComponenteCurricularId.HasValue && componenteCurricular.Codigo.Equals(filtro.ComponenteCurricularId.Value)) ? filtro.ConceitoId : null;

                            if (componenteCurricular.Regencia)
                            {
                                var componentesRegencia = await mediator
                                    .Send(new ObterComponentesCurricularesRegenciaPorTurmaQuery(turma, long.Parse(componenteCurricular.Codigo)));

                                foreach (var regencia in componentesRegencia)
                                {
                                    await SalvarConsolidacaoConselhoClasseNota(turma, filtro.Bimestre, regencia.Codigo, long.Parse(componenteCurricular.Codigo),
                                                                               filtro.AlunoCodigo, nota, conceitoId, consolidadoTurmaAlunoId);
                                }
                                continue;
                            }
                            await SalvarConsolidacaoConselhoClasseNota(turma, filtro.Bimestre, long.Parse(componenteCurricular.Codigo), 0,
                                                                       filtro.AlunoCodigo, nota, conceitoId, consolidadoTurmaAlunoId);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na persistência da consolidação do conselho de classe da turma aluno/nota", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message, "SGP", "ConselhoClasseConsolidado", ex.InnerException.ToString()));
                return false;
            }
        }

        private async Task<bool> SalvarConsolidacaoConselhoClasseNota(Turma turma, int? bimestre, long componenteCurricularId, long? componenteCurricularRegencia, string alunoCodigo, double? notaFiltro, long? conceitoFiltro, long consolidadoTurmaAlunoId)
        {
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdTurmaQuery(turma.Id, bimestre));
            IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotasAluno = null;
            IEnumerable<NotaConceitoBimestreComponenteDto> conselhoClasseNotasAluno = null;
            if (fechamentoTurma != null)
            {
                var conselhoClasse = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id));
                var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();

                if (componenteCurricularRegencia > 0)
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(turma.CodigoTurma, (long)componenteCurricularRegencia, bimestre));
                else
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(turma.CodigoTurma, componenteCurricularId, bimestre));

                if (fechamentoTurmaDisciplina != null)
                {
                    var arrayfechamentoTurmaDisciplinaId = new long[] { fechamentoTurmaDisciplina.Id };
                    fechamentoNotasAluno = await mediator.Send(new ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery(arrayfechamentoTurmaDisciplinaId, alunoCodigo));
                }

                if (conselhoClasse != null)
                    conselhoClasseNotasAluno = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhoClasse.Id, alunoCodigo, bimestre ?? 0, componenteCurricularId));
            }

            double? nota = null;
            double? conceito = null;
            if (conselhoClasseNotasAluno != null && conselhoClasseNotasAluno.Any(x => x.ComponenteCurricularCodigo == componenteCurricularId))
            {
                nota = conselhoClasseNotasAluno.First(x => x.ComponenteCurricularCodigo == componenteCurricularId).Nota;
                conceito = conselhoClasseNotasAluno.First(x => x.ComponenteCurricularCodigo == componenteCurricularId).ConceitoId;
            }
            else if (fechamentoNotasAluno != null && fechamentoNotasAluno.Any(x => x.ComponenteCurricularId == componenteCurricularId))
            {
                nota = fechamentoNotasAluno.First(x => x.ComponenteCurricularId == componenteCurricularId).Nota;
                conceito = fechamentoNotasAluno.First(x => x.ComponenteCurricularId == componenteCurricularId).ConceitoId;
            }

            var consolidadoNota = await repositorioConselhoClasseConsolidadoNota.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(consolidadoTurmaAlunoId, bimestre, componenteCurricularId);
            consolidadoNota ??= new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                Bimestre = bimestre,
            };

            consolidadoNota.ComponenteCurricularId = componenteCurricularId;
            consolidadoNota.Nota = (notaFiltro != null ? notaFiltro : nota);
            consolidadoNota.ConceitoId = (long?)(conceitoFiltro != null ? conceitoFiltro : conceito);

            await repositorioConselhoClasseConsolidadoNota.SalvarAsync(consolidadoNota);
            return true;
        }
    }
}
