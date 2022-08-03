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

            SituacaoConselhoClasse statusNovo = SituacaoConselhoClasse.NaoIniciado;

            var consolidadoTurmaAluno = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(filtro.TurmaId, filtro.AlunoCodigo);

            if (consolidadoTurmaAluno == null)
            {
                consolidadoTurmaAluno = new ConselhoClasseConsolidadoTurmaAluno
                {
                    AlunoCodigo = filtro.AlunoCodigo,
                    TurmaId = filtro.TurmaId,
                    Status = statusNovo
                };
            }

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdTurmaQuery(filtro.TurmaId));
            IEnumerable<FechamentoNotaAlunoAprovacaoDto> fechamentoNotasAluno = null;
            IEnumerable<NotaConceitoBimestreComponenteDto> conselhoClasseNotasAluno = null;
            if (fechamentoTurma != null)
            {
                var conselhoClasseId = await mediator.Send(new ObterConselhoClassePorFechamentoIdQuery(fechamentoTurma.Id));
                var fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(turma.CodigoTurma, (long)filtro.ComponenteCurricularId));
                
                if (fechamentoTurmaDisciplina != null)
                { 
                var arrayfechamentoTurmaDisciplinaId = new long[] { fechamentoTurmaDisciplina.Id };
                fechamentoNotasAluno = await mediator.Send(new ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery(arrayfechamentoTurmaDisciplinaId, filtro.AlunoCodigo));
                }

                if (conselhoClasseId != null)
                {
                    conselhoClasseNotasAluno = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhoClasseId.Id, filtro.AlunoCodigo, filtro.ComponenteCurricularId));
                }

            }

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

                double? nota = null;
                double? conceito = null;
                if (conselhoClasseNotasAluno.Any())
                {
                    nota = conselhoClasseNotasAluno.First().Nota;
                    conceito = conselhoClasseNotasAluno.First().ConceitoId;
                }
                else if (fechamentoNotasAluno.Any())
                {
                    nota = fechamentoNotasAluno.First().Nota;
                    conceito = fechamentoNotasAluno.First().ConceitoId;
                }

                //Quando parecer conclusivo, não altera a nota, atualiza somente o parecerId
                if (filtro.ComponenteCurricularId.HasValue && ((filtro.Nota != null || filtro.ConceitoId != null) || (nota != null || conceito != null)))
                {
                    var consolidadoNota = await repositorioConselhoClasseConsolidadoNota.ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoNotaAsync(consolidadoTurmaAlunoId, filtro.Bimestre, filtro.ComponenteCurricularId);
                    if (consolidadoNota == null) 
                        consolidadoNota = new ConselhoClasseConsolidadoTurmaAlunoNota() 
                        { 
                            ConselhoClasseConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId,
                            Bimestre = filtro.Bimestre
                        };
                    
                    consolidadoNota.ComponenteCurricularId = filtro.ComponenteCurricularId;
                    consolidadoNota.Nota = (long?)(filtro.Nota != null ? filtro.Nota: nota);
                    consolidadoNota.ConceitoId = (long?)(filtro.ConceitoId != null ? filtro.ConceitoId : conceito);

                    await repositorioConselhoClasseConsolidadoNota.SalvarAsync(consolidadoNota);
                }


                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Ocorreu um erro na persistência da consolidação do conselho de classe da turma aluno/nota", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
