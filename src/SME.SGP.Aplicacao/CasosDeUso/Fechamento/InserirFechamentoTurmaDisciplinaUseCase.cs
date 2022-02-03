using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFechamentoTurmaDisciplinaUseCase : AbstractUseCase, IInserirFechamentoTurmaDisciplinaUseCase
    {
        public InserirFechamentoTurmaDisciplinaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<FechamentoTurmaDisciplinaDto> Executar(FechamentoFinalTurmaDisciplinaDto fechamentoTurma, bool componenteSemNota = false)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = await ObterTurma(fechamentoTurma.TurmaId);
            var componenteCurricular = await ObterComponenteCurricular(fechamentoTurma.DisciplinaId);

            var fechamentoTurmaDisciplina = await MapearParaEntidade(fechamentoTurma.Id, fechamentoTurma, turma);

            //fechamento final
            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);
            var tipoNota = await mediator.Send(new ObterTipoNotaPorTurmaIdQuery(turma.Id, turma.TipoTurma));

            //fechamento bimestre
            if (fechamentoTurma?.Justificativa != null)
            {
                int tamanhoJustificativa = fechamentoTurma.Justificativa.Length;
                int limite = int.Parse(FechamentoTurmaDisciplinaEnum.TamanhoCampoJustificativa.Description());
                if (tamanhoJustificativa > limite)
                    throw new NegocioException("Justificativa não pode ter mais que " + limite.ToString() + " caracteres");
            }
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio,
                                                                                                           turma.AnoLetivo, turma.Semestre));
            var ue = turma.Ue;

            var periodos = await ObterPeriodoEscolarFechamentoReabertura(tipoCalendario, ue, fechamentoTurma.Bimestre);
            PeriodoEscolar periodoEscolar = periodos.periodoEscolar;
            if (periodoEscolar == null)
                throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {fechamentoTurma.Bimestre}º Bimestre");

            await CarregaFechamentoTurma(fechamentoTurmaDisciplina, turma, periodoEscolar);

            var parametroAlteracaoNotaFechamento = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, turma.AnoLetivo));

            // Valida Permissão do Professor na Turma/Disciplina            
            if (!turma.EhTurmaEdFisicaOuItinerario() && !usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                await VerificaSeProfessorPodePersistirTurma(usuarioLogado.CodigoRf, fechamentoTurma.TurmaId, periodoEscolar.PeriodoFim, periodos.periodoFechamento, fechamentoTurma.DisciplinaId.ToString(), usuarioLogado);

            var fechamentoAlunos = Enumerable.Empty<FechamentoAluno>();

            var disciplinaEOL = await mediator.Send(new ObterDisciplinasPorIdsQuery(new long[] { fechamentoTurmaDisciplina.DisciplinaId }));
            if (disciplinaEOL == null)
                throw new NegocioException("Não foi possível localizar o componente curricular no EOL.");

            // reprocessar do fechamento de componente sem nota deve atualizar a sintise de frequencia
            if (componenteSemNota && fechamentoTurma.Id > 0)
                fechamentoAlunos = await AtualizaSinteseAlunos(fechamentoTurma.Id, periodoEscolar.PeriodoFim, disciplinaEOL.FirstOrDefault(), turma.AnoLetivo, turma.CodigoTurma);
            else
                fechamentoAlunos = await CarregarFechamentoAlunoENota(fechamentoTurma.Id, fechamentoTurma.NotaConceitoAlunos, usuarioLogado, parametroAlteracaoNotaFechamento, turma.AnoLetivo);

            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));
            var parametroDiasAlteracao = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasAlteracaoNotaFinal, turma.AnoLetivo));
            var diasAlteracao = DateTime.Today.DayOfYear - fechamentoTurmaDisciplina.CriadoEm.Date.DayOfYear;
            var acimaDiasPermitidosAlteracao = parametroDiasAlteracao != null && diasAlteracao > int.Parse(parametroDiasAlteracao);
            var alunosComNotaAlterada = "";

            var fechamentoTurmaId = fechamentoTurmaDisciplina.FechamentoTurma.Id > 0 ?
                    fechamentoTurmaDisciplina.FechamentoTurma.Id : 
                    await mediator.Send(new SalvarFechamentoTurmaCommand(fechamentoTurmaDisciplina.FechamentoTurma));
            
            fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurmaId;
        }

        private async Task<Turma> ObterTurma(string turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            if (turma == null)
                throw new NegocioException("Não foi encontrada a turma informada.");

            return turma;
        }

        private async Task<bool> ExigeAprovacao(Turma turma, Usuario usuarioLogado)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaConselho, anoLetivo));
            if (parametro == null)
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotaConselho' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularId }));

            if (!componentes.Any())
                throw new NegocioException($"Componente Curricular do Fechamento ({componenteCurricularId}) não localizado!");

            return componentes.FirstOrDefault();
        }

        private async Task<FechamentoTurmaDisciplina> MapearParaEntidade(long id, FechamentoFinalTurmaDisciplinaDto fechamentoDto, Turma turma)
        {
            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;
      
            if (fechamentoDto.EhFinal)
            {
                //colocar verificar regencia
                var disciplinaId = fechamentoDto.EhRegencia ? fechamentoDto.DisciplinaId : fechamentoDto.NotaConceitoAlunos.First().DisciplinaId;

                var fechamentoFinalTurma = await mediator.Send(new ObterFechamentoTurmaPorTurmaIdQuery(turma.Id));
                if (fechamentoFinalTurma == null)
                    fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
                else
                    //verificar retorno e disciplina regencia
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaQuery(fechamentoDto.TurmaId, disciplinaId));

                if (fechamentoTurmaDisciplina == null)
                    fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina() { DisciplinaId = disciplinaId, Situacao = SituacaoFechamento.ProcessadoComSucesso };

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

                foreach (var agrupamentoAluno in fechamentoDto.NotaConceitoAlunos.GroupBy(a => a.CodigoAluno))
                {
                    var fechamentoAluno = await mediator.Send(new ObterFechamentoAlunoPorTurmaIdQuery(fechamentoTurmaDisciplina.Id, agrupamentoAluno.Key));
                    if (fechamentoAluno == null)
                        fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoAluno.Key };

                    fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
                }
            }
            else
            {
                if (id > 0)
                {
                    fechamentoTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorIdQuery(id));
                    fechamentoTurmaDisciplina.FechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorTurmaIdQuery(turma.Id));
                }

                fechamentoTurmaDisciplina.AtualizarSituacao(SituacaoFechamento.EmProcessamento);
                fechamentoTurmaDisciplina.DisciplinaId = fechamentoDto.DisciplinaId;
                fechamentoTurmaDisciplina.Justificativa = fechamentoDto.Justificativa;
            }

            return fechamentoTurmaDisciplina;
        }

        private async Task<(PeriodoEscolar periodoEscolar, PeriodoDto periodoFechamento)> ObterPeriodoEscolarFechamentoReabertura(long tipoCalendarioId, Ue ue, int bimestre)
        {
            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdQuery(tipoCalendarioId));
            var periodoFechamentoBimestre = periodoFechamento?.FechamentosBimestres.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoFechamento == null || periodoFechamentoBimestre == null)
            {
                var hoje = DateTime.Today;
                //ficar de olho no retorno
                var tipodeEventoReabertura = await mediator.Send(new ObterEventoTipoIdPorCodigoQuery(TipoEvento.FechamentoBimestre));

                if (await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(hoje, tipoCalendarioId, (TipoEvento)tipodeEventoReabertura, ue.CodigoUe, ue.Dre.CodigoDre)))
                {
                    var fechamentoReabertura = await mediator.Send(new ObterTurmaEmPeriodoFechamentoQuery(bimestre, hoje, tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe));
                    if (fechamentoReabertura == null)
                        throw new NegocioException($"Não localizado período de fechamento em aberto para turma informada no {bimestre}º Bimestre");

                    return ((await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId))).FirstOrDefault(a => a.Bimestre == bimestre)
                        , new PeriodoDto(fechamentoReabertura.Inicio, fechamentoReabertura.Fim));
                }
            }

            return (periodoFechamentoBimestre?.PeriodoEscolar
                , periodoFechamentoBimestre is null ?
                    null :
                    new PeriodoDto(periodoFechamentoBimestre.InicioDoFechamento.Value, periodoFechamentoBimestre.FinalDoFechamento.Value));
        }

        private async Task CarregaFechamentoTurma(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurmaDisciplina.Id > 0)
            {
                // Alterando registro de fechamento
                fechamentoTurmaDisciplina.FechamentoTurma.Turma = turma;
                fechamentoTurmaDisciplina.FechamentoTurma.TurmaId = turma.Id;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolar = periodoEscolar;
                fechamentoTurmaDisciplina.FechamentoTurma.PeriodoEscolarId = periodoEscolar.Id;
            }
            else
            {
                // Incluindo registro de fechamento turma disciplina

                // Busca registro existente de fechamento da turma
                var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorPeriodoQuery(turma.Id, periodoEscolar.Id));
                if (fechamentoTurma == null)
                    fechamentoTurma = new FechamentoTurma(turma, periodoEscolar);

                fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
            }
        }

        private async Task VerificaSeProfessorPodePersistirTurma(string codigoRf, string turmaId, DateTime dataAula, PeriodoDto periodoFechamento, string disciplinaId, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var podePersistir = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(Int64.Parse(disciplinaId), turmaId, dataAula, usuario));

            if (!usuario.EhProfessorCj() && !podePersistir)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }

        private async Task<IEnumerable<FechamentoAluno>> AtualizaSinteseAlunos(long fechamentoTurmaDisciplinaId, DateTime dataReferencia, DisciplinaDto disciplina, int anoLetivo, string codigoTurma)
        {
            var fechamentoAlunos = await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId));
            foreach (var fechamentoAluno in fechamentoAlunos)
            {
                foreach (var fechamentoNota in fechamentoAluno.FechamentoNotas)
                {
                    var frequencia = await mediator.Send(new ObterFrequenciaPorAlunoDisciplinaDataQuery(fechamentoAluno.AlunoCodigo, fechamentoNota.DisciplinaId.ToString(), dataReferencia, codigoTurma));
                    var percentualFrequencia = frequencia == null ? 100 : frequencia.PercentualFrequencia;
                    var sinteseDto = await ObterSinteseAluno(percentualFrequencia, disciplina, anoLetivo);

                    fechamentoNota.SinteseId = (long)sinteseDto.Id;
                }
            }

            return fechamentoAlunos;
        }

        public async Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina, int anoLetivo)
        {
            var sintese = percentualFrequencia == null ?
                SinteseEnum.NaoFrequente :
                percentualFrequencia >= await ObterFrequenciaMedia(disciplina, anoLetivo) ?
                SinteseEnum.Frequente :
                SinteseEnum.NaoFrequente;

            return new SinteseDto()
            {
                Id = sintese,
                Valor = sintese.Name()
            };
        }

        public async Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina, int anoLetivo)
        {
            double mediaFrequencia = 0;
            
            if (mediaFrequencia == 0)
            {
                if (disciplina.Regencia || !disciplina.LancaNota)
                    mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, anoLetivo)));
                else
                    mediaFrequencia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualFund2, anoLetivo)));
            }

            return mediaFrequencia;
        }

        private async Task<IEnumerable<FechamentoAluno>> CarregarFechamentoAlunoENota(long fechamentoTurmaDisciplinaId, IEnumerable<FechamentoNotaDto> fechamentoNotasDto, Usuario usuarioLogado, ParametrosSistema parametroAlteracaoNotaFechamento, int turmaAnoLetivo)
        {
            var fechamentoAlunos = new List<FechamentoAluno>();

            if (fechamentoTurmaDisciplinaId > 0)
                fechamentoAlunos = (await mediator.Send(new ObterFechamentoAlunoPorDisciplinaIdQuery(fechamentoTurmaDisciplinaId))).ToList();

            foreach (var agrupamentoNotasAluno in fechamentoNotasDto.GroupBy(g => g.CodigoAluno))
            {
                var fechamentoAluno = fechamentoAlunos.FirstOrDefault(c => c.AlunoCodigo == agrupamentoNotasAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoNotasAluno.Key, FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId };

                foreach (var fechamentoNotaDto in agrupamentoNotasAluno)
                {
                    var notaFechamento = fechamentoAluno.FechamentoNotas.FirstOrDefault(x => x.DisciplinaId == fechamentoNotaDto.DisciplinaId);
                    if (notaFechamento != null)
                    {
                        if (!notaFechamento.ConceitoId.HasValue)
                        {
                            if (fechamentoNotaDto.Nota != notaFechamento.Nota)
                                await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(notaFechamento.Nota != null ? notaFechamento.Nota.Value : (double?)null, fechamentoNotaDto.Nota != null ? fechamentoNotaDto.Nota.Value : (double?)null, notaFechamento.Id));
                        }
                        else
                        {
                            if (fechamentoNotaDto.ConceitoId != notaFechamento.ConceitoId)
                                await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(notaFechamento.ConceitoId != null ? notaFechamento.ConceitoId.Value : (long?)null, fechamentoNotaDto.ConceitoId != null ? fechamentoNotaDto.ConceitoId.Value : (long?)null, notaFechamento.Id));
                        }

                        if (EnviarWfAprovacao(usuarioLogado, turmaAnoLetivo) && parametroAlteracaoNotaFechamento.Ativo)
                        {
                            fechamentoNotaDto.Id = notaFechamento.Id;
                            if (!notaFechamento.ConceitoId.HasValue)
                                fechamentoNotaDto.NotaAnterior = notaFechamento.Nota != null ? notaFechamento.Nota.Value : (double?)null;
                            else
                                fechamentoNotaDto.ConceitoIdAnterior = notaFechamento.ConceitoId != null ? notaFechamento.ConceitoId.Value : (long?)null;

                            //notasEnvioWfAprovacao.Add(fechamentoNotaDto);
                        }
                        else
                        {
                            notaFechamento.Nota = fechamentoNotaDto.Nota;
                            notaFechamento.ConceitoId = fechamentoNotaDto.ConceitoId;
                            notaFechamento.SinteseId = fechamentoNotaDto.SinteseId;
                        }
                    }
                    else
                        fechamentoAluno.AdicionarNota(MapearParaEntidade(fechamentoNotaDto));
                }
                fechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoAlunos;
        }

        private bool EnviarWfAprovacao(Usuario usuarioLogado, int turmaAnoLetivo)
        {
            if (turmaAnoLetivo != DateTime.Today.Year && !usuarioLogado.EhGestorEscolar())
                return true;

            return false;
        }

        private FechamentoNota MapearParaEntidade(FechamentoNotaDto fechamentoNotaDto)
            => fechamentoNotaDto == null ? null :
              new FechamentoNota()
              {
                  DisciplinaId = fechamentoNotaDto.DisciplinaId,
                  Nota = fechamentoNotaDto.Nota,
                  ConceitoId = fechamentoNotaDto.ConceitoId,
                  SinteseId = fechamentoNotaDto.SinteseId
              };
    }
}
