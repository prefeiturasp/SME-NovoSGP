﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarFechamentoTurmaBimestreUseCase : IListarFechamentoTurmaBimestreUseCase
    {
        private readonly IMediator mediator;

        public ListarFechamentoTurmaBimestreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<FechamentoNotaConceitoTurmaDto> Executar(string turmaCodigo, long componenteCurricularCodigo, int bimestre, int? semestre)
        {
            var fechamentoNotaConceitoTurma = new FechamentoNotaConceitoTurmaDto();

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turmaCodigo));
            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");

            var tipoNotaTurma = await mediator.Send(new ObterTipoNotaPorTurmaIdQuery(turma.Id, turma.TipoTurma));
            if (tipoNotaTurma == null)
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), semestre.HasValue ? semestre.Value : 0));
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { componenteCurricularCodigo }));
            if (!componentesCurriculares.Any())
                throw new NegocioException("Não foi possível localizar dados do componente curricular selecionado.");

            fechamentoNotaConceitoTurma.PercentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            var componenteCurricularSelecionado = componentesCurriculares.FirstOrDefault();
            var usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var bimestreAtual = bimestre;

            fechamentoNotaConceitoTurma.NotaTipo = tipoNotaTurma.TipoNota;
            fechamentoNotaConceitoTurma.MediaAprovacaoBimestre = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));

            var disciplinas = new List<DisciplinaDto>();
            IEnumerable<DisciplinaDto> disciplinasRegencia = null;

            if (componenteCurricularSelecionado.Regencia)
            {
                disciplinasRegencia = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(turmaCodigo));

                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                if (turma.EhEJA() && disciplinasRegencia != null)
                    disciplinasRegencia = disciplinasRegencia.Where(n => n.CodigoComponenteCurricular != 6);

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaDto() { Nome = componenteCurricularSelecionado.Nome, CodigoComponenteCurricular = componenteCurricularSelecionado.CodigoComponenteCurricular });

            var alunosComAnotacao = Enumerable.Empty<string>();
            var fechamentosTurma = await ObterFechamentosTurmaDisciplina(turmaCodigo, componenteCurricularCodigo.ToString(), bimestre);
            if (fechamentosTurma != null && fechamentosTurma.Any())
            {
                fechamentoNotaConceitoTurma.FechamentoId = fechamentosTurma.First().Id;
                fechamentoNotaConceitoTurma.DataFechamento = fechamentosTurma.First().AlteradoEm.HasValue ? fechamentosTurma.First().AlteradoEm.Value : fechamentosTurma.First().CriadoEm;
                fechamentoNotaConceitoTurma.Situacao = fechamentosTurma.First().Situacao;
                fechamentoNotaConceitoTurma.SituacaoNome = fechamentosTurma.First().Situacao.Name();

                alunosComAnotacao = await ObterAlunosComAnotacaoNoFechamento(fechamentoNotaConceitoTurma.FechamentoId);
            }
            else
            {
                fechamentoNotaConceitoTurma.Situacao = SituacaoFechamento.NaoIniciado;
                fechamentoNotaConceitoTurma.SituacaoNome = SituacaoFechamento.NaoIniciado.Name();
            }

            IOrderedEnumerable<AlunoPorTurmaResposta> alunosValidosComOrdenacao = null;
            if (bimestre > 0)
            {
                var tipoAvaliacaoBimestral = await mediator.Send(new ObterTipoAvaliacaoBimestralQuery());
                await ValidaMinimoAvaliacoesBimestrais(componenteCurricularSelecionado, disciplinasRegencia, tipoCalendario.Id, turma.CodigoTurma, bimestreAtual, tipoAvaliacaoBimestral, fechamentoNotaConceitoTurma);
                var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
                if (periodoAtual == null)
                    throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");
                fechamentoNotaConceitoTurma.PeriodoFim = periodoAtual.PeriodoFim;

                var bimestreDoPeriodo = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(tipoCalendario.Id, periodoAtual.PeriodoFim));

                alunosValidosComOrdenacao = alunos.Where(a => a.DeveMostrarNaChamada(bimestreDoPeriodo.PeriodoFim, bimestreDoPeriodo.PeriodoInicio))
                                                      .OrderBy(a => a.NomeAluno)
                                                      .ThenBy(a => a.NomeValido());

                var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, componenteCurricularCodigo.ToString(), bimestreDoPeriodo.Id));

                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoBimestreEspecifico(alunosValidosComOrdenacao, fechamentosTurma, periodoAtual, turma,
                                                           componenteCurricularCodigo.ToString(), turmaPossuiFrequenciaRegistrada, componenteCurricularSelecionado, disciplinasRegencia,
                                                           periodosEscolares, usuarioAtual, alunosComAnotacao);

                fechamentoNotaConceitoTurma.PossuiAvaliacao = await mediator.Send(new TurmaPossuiAvaliacaoNoPeriodoQuery(turma.Id, periodoAtual.Id, componenteCurricularCodigo));
                fechamentoNotaConceitoTurma.PeriodoEscolarId = periodoAtual.Id;
            }
            else if (bimestre == 0)
            {
                fechamentoNotaConceitoTurma.PeriodoFim = ultimoPeriodoEscolar.PeriodoFim;

                alunosValidosComOrdenacao = alunos.Where(a => a.DeveMostrarNaChamada(ultimoPeriodoEscolar.PeriodoFim, ultimoPeriodoEscolar.PeriodoInicio))
                                                  .OrderBy(a => a.NomeAluno)
                                                  .ThenBy(a => a.NomeValido());

                fechamentoNotaConceitoTurma.Alunos = await RetornaListagemAlunosFechamentoFinal(alunosValidosComOrdenacao, disciplinas, fechamentosTurma, turma,
                                                            componenteCurricularCodigo, periodosEscolares, tipoNotaTurma, usuarioAtual, alunosComAnotacao);
            }

            fechamentoNotaConceitoTurma.AuditoriaAlteracao = MontaTextoAuditoriaAlteracao(fechamentosTurma.Any() ? fechamentosTurma.FirstOrDefault() : null, tipoNotaTurma.EhNota());
            fechamentoNotaConceitoTurma.AuditoriaInclusao = MontaTextoAuditoriaInclusao(fechamentosTurma.Any() ? fechamentosTurma.FirstOrDefault() : null, tipoNotaTurma.EhNota());

            return fechamentoNotaConceitoTurma;
        }

        private Task<IEnumerable<string>> ObterAlunosComAnotacaoNoFechamento(long fechamentoId)
            => mediator.Send(new ObterCodigosAlunosComAnotacaoNoFechamentoQuery(fechamentoId));

        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoBimestreEspecifico(IEnumerable<AlunoPorTurmaResposta> alunos,
            IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma, PeriodoEscolar periodoAtual, Turma turma, string componenteCurricularCodigo,
            bool turmaPossuiFrequenciaRegistrada, DisciplinaDto disciplina, IEnumerable<DisciplinaDto> disciplinasRegencia, IEnumerable<PeriodoEscolar> periodosEscolares
            , Usuario usuarioAtual, IEnumerable<string> alunosComAnotacao)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, periodoAtual, turma, componenteCurricularCodigo.ToString(), periodoAtual.PeriodoInicio);
            var exigeAprovacao = await mediator.Send(new ExigeAprovacaoDeNotaQuery(turma));

            foreach (var aluno in alunos)
            {
                var fechamentoTurma = (from ft in fechamentosTurma
                                       from fa in ft.FechamentoAlunos
                                       where fa.AlunoCodigo.Equals(aluno.CodigoAluno)
                                       select ft).FirstOrDefault();

                var alunoDto = new AlunosFechamentoNotaConceitoTurmaDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                    Nome = aluno.NomeAluno,
                    EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
                };

                alunoDto.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, periodoAtual.PeriodoInicio, turma.EhTurmaInfantil));
                alunoDto.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceitoNoPeriodo(periodoAtual, usuarioEPeriodoPodeEditar) : false;

                var frequenciaAluno = await mediator.Send(new ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery(aluno.CodigoAluno, componenteCurricularCodigo, periodoAtual.Id, TipoFrequenciaAluno.PorDisciplina, turma.CodigoTurma));
                if (frequenciaAluno != null)
                    alunoDto.Frequencia = frequenciaAluno.PercentualFrequencia.ToString();
                else
                    alunoDto.Frequencia = turmaPossuiFrequenciaRegistrada ? "100" : string.Empty;

                if (aluno.CodigoAluno != null)
                {
                    var notasConceitoBimestre = await ObterNotasBimestre(aluno.CodigoAluno, fechamentoTurma != null ? fechamentoTurma.Id : 0);

                    if (notasConceitoBimestre.Any())
                        alunoDto.NotasConceitoBimestre = new List<FechamentoConsultaNotaConceitoTurmaListaoDto>();

                    if (turma.EhEJA() && notasConceitoBimestre != null)
                        notasConceitoBimestre = notasConceitoBimestre.Where(n => n.DisciplinaId != 6);

                    if (notasConceitoBimestre.Any())
                    {
                        foreach (var notaConceitoBimestre in notasConceitoBimestre)
                        {
                            string nomeDisciplina;
                            if (disciplina.Regencia)
                                nomeDisciplina = disciplinasRegencia.FirstOrDefault(a => a.CodigoComponenteCurricular == notaConceitoBimestre.DisciplinaId)?.Nome;
                            else nomeDisciplina = disciplina.Nome;

                            double? notaConceito;

                            if (notaConceitoBimestre.ConceitoId.HasValue)
                            {
                                var valorConceito = await ObterConceito(notaConceitoBimestre.ConceitoId.Value);
                                notaConceito = valorConceito;
                            }
                            else
                                notaConceito = notaConceitoBimestre.Nota;

                            var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                            {
                                Bimestre = periodoAtual.Bimestre,
                                DisciplinaCodigo = notaConceitoBimestre.DisciplinaId,
                                Disciplina = nomeDisciplina,
                                NotaConceito = notaConceito
                            };

                            if (exigeAprovacao)
                                await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                            ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                        }

                        if (disciplina.Regencia)
                        {
                            var listaDisciplinasSemNotaAluno = disciplinasRegencia.Where(d => !notasConceitoBimestre.Select(n => n.DisciplinaId).Contains(d.CodigoComponenteCurricular));

                            if (listaDisciplinasSemNotaAluno.Any())
                            {
                                foreach (var disciplinasSemNota in listaDisciplinasSemNotaAluno)
                                {
                                    var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                                    {
                                        Bimestre = periodoAtual.Bimestre,
                                        DisciplinaCodigo = disciplinasSemNota.CodigoComponenteCurricular,
                                        Disciplina = disciplinasSemNota.Nome
                                    };

                                    ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (disciplinasRegencia != null)
                        {
                            foreach (var disciplinaReg in disciplinasRegencia)
                            {
                                var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                                {
                                    Bimestre = periodoAtual.Bimestre,
                                    DisciplinaCodigo = disciplinaReg.CodigoComponenteCurricular,
                                    Disciplina = disciplinaReg.Nome
                                };

                                if (fechamentoTurma != null && nota.DisciplinaCodigo > 0)
                                    await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                                ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                            }
                        }
                        else
                        {
                            var nota = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                            {
                                Bimestre = periodoAtual.Bimestre,
                                DisciplinaCodigo = disciplina.Id,
                                Disciplina = disciplina.Nome
                            };

                            if (fechamentoTurma != null && nota.DisciplinaCodigo > 0)
                                await VerificaNotaEmAprovacao(aluno.CodigoAluno, fechamentoTurma.FechamentoTurmaId, nota.DisciplinaCodigo, nota);

                            ((List<FechamentoConsultaNotaConceitoTurmaListaoDto>)alunoDto.NotasConceitoBimestre).Add(nota);
                        }
                    }

                    alunoDto.PossuiAnotacao = alunosComAnotacao.Any(a => a == aluno.CodigoAluno);

                    if (alunoDto.NotasConceitoBimestre.Any())
                        alunoDto.NotasConceitoBimestre = alunoDto.NotasConceitoBimestre.OrderBy(a => a.Disciplina).ToList();

                    alunosFechamentoNotaConceito.Add(alunoDto);
                }
            }

            return alunosFechamentoNotaConceito;
        }

        public async Task<IList<AlunosFechamentoNotaConceitoTurmaDto>> RetornaListagemAlunosFechamentoFinal(IEnumerable<AlunoPorTurmaResposta> alunos, List<DisciplinaDto> disciplinas,
            IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma, Turma turma, long componenteCurricularCodigo, IEnumerable<PeriodoEscolar> periodosEscolares,
            NotaTipoValor tipoNota, Usuario usuarioAtual, IEnumerable<string> alunosComAnotacao)
        {
            var alunosFechamentoNotaConceito = new List<AlunosFechamentoNotaConceitoTurmaDto>();

            if (turma.AnoLetivo != 2020)
                await VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma);

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();
            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, ultimoPeriodoEscolar, turma, componenteCurricularCodigo.ToString(), ultimoPeriodoEscolar.PeriodoInicio);
            var notasFechamentosBimestres = await ObterNotasFechamentosBimestres(componenteCurricularCodigo, turma, periodosEscolares, tipoNota.EhNota());

            var notasFechamentosFinais = Enumerable.Empty<FechamentoNotaAlunoAprovacaoDto>();
            if (fechamentosTurma != null && fechamentosTurma.Any())
                notasFechamentosFinais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosTurma.Select(ftd => ftd.Id).ToArray(), turma.CodigoTurma, componenteCurricularCodigo.ToString()));

            foreach (var aluno in alunos)
            {
                AlunosFechamentoNotaConceitoTurmaDto fechamentoFinalAluno = await TrataFrequenciaAluno(componenteCurricularCodigo.ToString(), aluno, turma);

                fechamentoFinalAluno.Marcador = await mediator.Send(new ObterMarcadorAlunoQuery(aluno, ultimoPeriodoEscolar.PeriodoInicio, turma.EhTurmaInfantil));

                foreach (var periodo in periodosEscolares.OrderBy(a => a.Bimestre))
                {
                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        var nota = notasFechamentosBimestres.FirstOrDefault(a => a.Bimestre == periodo.Bimestre
                                                                            && a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                            && a.AlunoCodigo == aluno.CodigoAluno);

                        var notaConceitoTurma = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                        {
                            Bimestre = periodo.Bimestre,
                            Disciplina = disciplinaParaAdicionar.Nome,
                            DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        };

                        if (nota != null)
                            if (!string.IsNullOrEmpty(nota.NotaConceito))
                                notaConceitoTurma.NotaConceito = double.Parse(nota.NotaConceito);

                        fechamentoFinalAluno.NotasConceitoBimestre.Add(notaConceitoTurma);
                    }
                }

                foreach (var disciplinaParaAdicionar in disciplinas)
                {
                    var nota = notasFechamentosFinais.FirstOrDefault(a => a.ComponenteCurricularId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                    && a.AlunoCodigo == aluno.CodigoAluno);

                    var notaConceitoTurma = new FechamentoConsultaNotaConceitoTurmaListaoDto()
                    {
                        Disciplina = disciplinaParaAdicionar.Nome,
                        DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                        EmAprovacao = nota?.EmAprovacao ?? false
                    };

                    if (nota != null)
                    {
                        var valorNota = tipoNota.EhNota() ? nota.Nota : nota.ConceitoId;

                        if (valorNota.HasValue)
                            notaConceitoTurma.NotaConceito = valorNota;
                    }
                    fechamentoFinalAluno.NotasConceitoFinal.Add(notaConceitoTurma);
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;
                fechamentoFinalAluno.CodigoAluno = aluno.CodigoAluno;

                fechamentoFinalAluno.PossuiAnotacao = alunosComAnotacao.Any(a => a == aluno.CodigoAluno);

                if (fechamentoFinalAluno.NotasConceitoBimestre.Any())
                    fechamentoFinalAluno.NotasConceitoBimestre = fechamentoFinalAluno.NotasConceitoBimestre.OrderBy(a => a.Disciplina).ToList();

                alunosFechamentoNotaConceito.Add(fechamentoFinalAluno);
            }



            return alunosFechamentoNotaConceito;
        }
        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplina(string turmaCodigo, string disciplinaId, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            return await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, new long[] { Convert.ToInt64(disciplinaId) }, bimestre));
        }

        private string MontaTextoAuditoriaAlteracao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            if (fechamentoTurmaDisciplina != null && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.AlteradoPor))
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "alteradas" : "alterados")} por {fechamentoTurmaDisciplina.AlteradoPor}({fechamentoTurmaDisciplina.AlteradoRF}) em {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("dd/MM/yyyy")}, às {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("HH:mm")}.";
            else return string.Empty;
        }

        private string MontaTextoAuditoriaInclusao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            var criadorRf = fechamentoTurmaDisciplina != null && fechamentoTurmaDisciplina.CriadoRF != "0" && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.CriadoRF) ?
                $"({fechamentoTurmaDisciplina.CriadoRF})" : "";

            if (fechamentoTurmaDisciplina != null)
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "incluídas" : "incluídos")} por {fechamentoTurmaDisciplina.CriadoPor}{criadorRf} em {fechamentoTurmaDisciplina.CriadoEm.ToString("dd/MM/yyyy")}, às {fechamentoTurmaDisciplina.CriadoEm.ToString("HH:mm")}.";

            else return string.Empty;
        }

        private async Task VerificaSePodeFazerFechamentoFinal(IEnumerable<PeriodoEscolar> periodosEscolares, Turma turma)
        {
            var ultimoBimestre = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().Bimestre;

            var fechamentoDoUltimoBimestre = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, null, ultimoBimestre));

            if (fechamentoDoUltimoBimestre == null || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoBimestre}º  bimestre.");
        }

        private async Task<bool> PodeEditarNotaOuConceitoPeriodoUsuario(Usuario usuarioLogado, PeriodoEscolar periodoEscolar, Turma turma, string codigoComponenteCurricular, DateTime data)
        {
            if (!usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilDRE() && !usuarioLogado.EhPerfilSME())
            {
                var usuarioPodeEditar = await mediator.Send(new PodePersistirTurmaDisciplinaQuery(usuarioLogado.CodigoRf, turma.CodigoTurma, codigoComponenteCurricular, data.Ticks));
                if (!usuarioPodeEditar)
                    return false;
            }

            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoFechamentoQuery(turma, periodoEscolar.Bimestre, DateTimeExtension.HorarioBrasilia().Date));
            var podeLancarNota = await mediator.Send(new ObterComponenteLancaNotaQuery(Convert.ToInt64(codigoComponenteCurricular)));

            return temPeriodoAberto && podeLancarNota;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();
            var fechamentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();
            IEnumerable<FechamentoNotaAlunoAprovacaoDto> notasBimestrais = new List<FechamentoNotaAlunoAprovacaoDto>();
            foreach (var periodo in periodosEscolares)
            {
                var fechamentoBimestreTurma = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery(turma.Id, new long[] { disciplinaCodigo }, periodo.Bimestre));

                if (fechamentoBimestreTurma.Any())
                    fechamentosTurmaDisciplina.AddRange(fechamentoBimestreTurma);
            }


            var fechamentosIds = fechamentosTurmaDisciplina?.Select(a => a.Id).ToArray() ?? new long[] { };

            if (fechamentosIds.Length > 0)
                notasBimestrais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosIds, turma.CodigoTurma, disciplinaCodigo.ToString()));

            foreach (var nota in notasBimestrais.Where(a => a.Bimestre.HasValue))
            {
                var notaParaAdicionar = ehNota ?
                                            nota?.Nota.ToString() :
                                            nota?.ConceitoId.ToString();

                listaRetorno.Add(new FechamentoNotaAlunoDto(nota.Bimestre.Value,
                                                            notaParaAdicionar,
                                                            nota.ComponenteCurricularId,
                                                            nota.AlunoCodigo));
            }

            return listaRetorno;
        }

        private async Task<AlunosFechamentoNotaConceitoTurmaDto> TrataFrequenciaAluno(string componenteCurricularCodigo, AlunoPorTurmaResposta aluno, Turma turma)
        {
            var frequenciaAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery(aluno.CodigoAluno, turma.CodigoTurma, componenteCurricularCodigo));

            var percentualFrequencia = frequenciaAluno?.PercentualFrequencia ?? 100;

            if (frequenciaAluno != null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var fechamentoFinalAluno = new AlunosFechamentoNotaConceitoTurmaDto
            {
                Nome = aluno.NomeAluno,
                Frequencia = percentualFrequencia.ToString(),
                NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
            };
            return fechamentoFinalAluno;
        }

        private async Task VerificaNotaEmAprovacao(string codigoAluno, long turmaFechamentoId, long disciplinaId, FechamentoConsultaNotaConceitoTurmaListaoDto notasConceito)
        {
            double nota = await mediator.Send(new ObterNotaEmAprovacaoQuery(codigoAluno, turmaFechamentoId, disciplinaId));

            if (nota >= 0)
            {
                notasConceito.NotaConceito = nota;
                notasConceito.EmAprovacao = true;
            }
            else
            {
                notasConceito.EmAprovacao = false;
            }
        }

        private async Task<double> ObterConceito(long id)
        {
            var conceito = await mediator.Send(new ObterConceitoPorIdQuery(id));
            return conceito != null ? conceito.Id : 0;
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
           => await mediator.Send(new ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery(codigoAluno, fechamentoTurmaId));

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }

        private async Task ValidaMinimoAvaliacoesBimestrais(DisciplinaDto disciplinaEOL, IEnumerable<DisciplinaDto> disciplinasRegencia, long tipoCalendarioId, string turmaCodigo, int bimestre, TipoAvaliacao tipoAvaliacaoBimestral, FechamentoNotaConceitoTurmaDto fechamentoNotaConceitoTurma)
        {
            if (disciplinaEOL.Regencia)
            {
                var disciplinasObservacao = new List<string>();
                foreach (var disciplinaRegencia in disciplinasRegencia)
                {
                    var avaliacoes = await mediator.Send(new ObterAvaliacoesBimestraisRegenciaQuery(tipoCalendarioId, turmaCodigo, disciplinaRegencia.CodigoComponenteCurricular.ToString(), bimestre));
                    if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                        disciplinasObservacao.Add(disciplinaRegencia.Nome);
                }
                if (disciplinasObservacao.Count > 0)
                    fechamentoNotaConceitoTurma.Observacoes.Add($"O(s) componente(s) curricular(es) [{string.Join(",", disciplinasObservacao)}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
            else
            {
                var avaliacoes = await mediator.Send(new ObterAvaliacoesBimestraisQuery(tipoCalendarioId, turmaCodigo, disciplinaEOL.CodigoComponenteCurricular.ToString(), bimestre));
                if ((avaliacoes == null) || (avaliacoes.Count() < tipoAvaliacaoBimestral.AvaliacoesNecessariasPorBimestre))
                    fechamentoNotaConceitoTurma.Observacoes.Add($"O componente curricular [{disciplinaEOL.Nome}] não tem o número mínimo de avaliações bimestrais no bimestre {bimestre}");
            }
        }

    }
}
