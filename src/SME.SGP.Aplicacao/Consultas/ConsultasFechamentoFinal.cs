﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;
        public ConsultasFechamentoFinal(IRepositorioTurmaConsulta repositorioTurma, IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
                            IServicoEol servicoEOL, IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
                            IServicoAluno servicoAluno,
                            IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor,
                            IServicoUsuario servicoUsuario, IRepositorioParametrosSistema repositorioParametrosSistema,
                            IConsultasDisciplina consultasDisciplina, IConsultasPeriodoFechamento consultasPeriodoFechamento,
                            IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                            IMediator mediator,
                            IRepositorioCache repositorioCache)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoNota));
            this.servicoAluno = servicoAluno ?? throw new System.ArgumentNullException(nameof(servicoAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new System.ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentNullException(nameof(consultasDisciplina));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(filtros.TurmaCodigo);

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), filtros.Semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            if (turma.AnoLetivo != 2020)
                await VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma);

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();

            retorno.EventoData = ultimoPeriodoEscolar.PeriodoInicio;

            var fechamentoDoUltimoBimestre = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, new long[] { filtros.DisciplinaCodigo }, ultimoPeriodoEscolar.Bimestre));

            if (fechamentoDoUltimoBimestre == null || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoPeriodoEscolar.Bimestre}º  bimestre.");
            
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma, turma.AnoLetivo));
            
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(turma.CodigoTurma));
            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(turma.Id, turmaEOL.TipoTurma);
            if (tipoNota == null)
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            retorno.EhNota = tipoNota.EhNota();
            //Codigo aluno / NotaConceito / Código Disciplina / bimestre

            var listaAlunosNotas = new List<(string, string, long, int)>();

            var disciplinas = new List<DisciplinaResposta>();
            var disciplinaEOL = await consultasDisciplina.ObterDisciplina(filtros.DisciplinaCodigo);
            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            if (filtros.EhRegencia)
            {
                var disciplinasRegencia = await consultasDisciplina.ObterComponentesRegencia(turma);

                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaResposta() { Nome = disciplinaEOL.Nome, CodigoComponenteCurricular = disciplinaEOL.CodigoComponenteCurricular });

            retorno.EhSintese = !disciplinaEOL.LancaNota;

            var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, new long[] { filtros.DisciplinaCodigo });
            var notasFechamentosFinais = Enumerable.Empty<FechamentoNotaAlunoAprovacaoDto>();

            if (fechamentosTurmaDisciplina != null && fechamentosTurmaDisciplina.Any())
                notasFechamentosFinais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosTurmaDisciplina.Select(ftd => ftd.Id).ToArray(), turma.CodigoTurma, filtros.DisciplinaCodigo.ToString()));

            var notasFechamentosBimestres = Enumerable.Empty<FechamentoNotaAlunoDto>();

            if (!retorno.EhSintese)
                notasFechamentosBimestres = await ObterNotasFechamentosBimestres(filtros.DisciplinaCodigo, turma, periodosEscolares, retorno.EhNota);


            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, ultimoPeriodoEscolar, turma, filtros.DisciplinaCodigo.ToString(), retorno.EventoData);
            var alunosValidosOrdenados = alunosDaTurma
                .Where(a => a.NumeroAlunoChamada > 0 ||
                            a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo) ||
                            a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido))
                .OrderBy(a => a.NumeroAlunoChamada)
                .ThenBy(a => a.NomeValido());

            foreach (var aluno in alunosValidosOrdenados)
            {
                FechamentoFinalConsultaRetornoAlunoDto fechamentoFinalAluno = await TrataFrequenciaAluno(filtros, periodosEscolares, aluno, turma);

                var marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolar() { PeriodoFim = retorno.EventoData });
                if (marcador != null)
                    fechamentoFinalAluno.Informacao = marcador.Descricao;

                if (retorno.EhSintese)
                {
                    var sinteseDto = await mediator.Send(new ObterSinteseAlunoQuery(fechamentoFinalAluno.FrequenciaValor, disciplinaEOL, turma.AnoLetivo));
                    fechamentoFinalAluno.Sintese = sinteseDto.Valor;
                }
                else
                {
                    foreach (var periodo in periodosEscolares.OrderBy(a => a.Bimestre))
                    {
                        foreach (var disciplinaParaAdicionar in disciplinas)
                        {
                            //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
                            var nota = notasFechamentosBimestres?.FirstOrDefault(a => a.Bimestre == periodo.Bimestre
                                                                                && a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular
                                                                                && a.AlunoCodigo == aluno.CodigoAluno);
                            var notaParaAdicionar = nota?.NotaConceito ?? "";


                            fechamentoFinalAluno.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                            {
                                Bimestre = periodo.Bimestre,
                                Disciplina = disciplinaParaAdicionar.Nome,
                                DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                                NotaConceito = notaParaAdicionar,

                            });

                        }
                    }

                    foreach (var disciplina in disciplinas)
                    {
                        var codigoComponenteCurricular = disciplina.CodigoComponenteCurricular;
                        var nota = notasFechamentosFinais?.FirstOrDefault(a => a.ComponenteCurricularId == codigoComponenteCurricular
                                                                        && a.AlunoCodigo == aluno.CodigoAluno);

                        string notaParaAdicionar = nota == null ? string.Empty :
                                                   tipoNota.EhNota() ? nota.Nota.HasValue ? nota.Nota.Value.ToString() : ""
                                                                     : nota.ConceitoId.HasValue ? nota.ConceitoId.Value.ToString() : "";

                        fechamentoFinalAluno.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Disciplina = disciplina.Nome,
                            DisciplinaCodigo = disciplina.CodigoComponenteCurricular,
                            NotaConceito = notaParaAdicionar,
                            EmAprovacao = nota?.EmAprovacao ?? false
                        });

                    }
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;
                fechamentoFinalAluno.Codigo = aluno.CodigoAluno;
                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            retorno.AuditoriaAlteracao = MontaTextoAuditoriaAlteracao(fechamentosTurmaDisciplina.Any() ? fechamentosTurmaDisciplina.FirstOrDefault() : null, retorno.EhNota);
            retorno.AuditoriaInclusao = MontaTextoAuditoriaInclusao(fechamentosTurmaDisciplina.Any() ? fechamentosTurmaDisciplina.FirstOrDefault() : null, retorno.EhNota);

            retorno.NotaMedia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, turma.AnoLetivo)));
            retorno.FrequenciaMedia = await mediator.Send(new ObterFrequenciaMediaQuery(disciplinaEOL, turma.AnoLetivo));
            retorno.PeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, ultimoPeriodoEscolar.Bimestre, turma.AnoLetivo == DateTime.Today.Year));

            return retorno;
        }

        private string MontaTextoAuditoriaAlteracao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            if (fechamentoTurmaDisciplina != null && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.AlteradoPor))
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "alteradas" : "alterados")} por {fechamentoTurmaDisciplina.AlteradoPor}({fechamentoTurmaDisciplina.AlteradoRF}) em {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("HH:mm")}.";
            else return string.Empty;
        }

        private string MontaTextoAuditoriaInclusao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            var criadorRf = fechamentoTurmaDisciplina != null && fechamentoTurmaDisciplina.CriadoRF != "0" && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.CriadoRF) ?
                $"({fechamentoTurmaDisciplina.CriadoRF})" : "";

            if (fechamentoTurmaDisciplina != null)
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "incluídas" : "incluídos")} por {fechamentoTurmaDisciplina.CriadoPor}{criadorRf} em {fechamentoTurmaDisciplina.CriadoEm.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.CriadoEm.ToString("HH:mm")}.";

            else return string.Empty;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();
            var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, new long[] { disciplinaCodigo }, -1);
            var fechamentosIds = fechamentosTurmaDisciplina?.Select(a => a.Id).ToArray() ?? new long[] { };
            var notasBimestrais = await repositorioFechamentoNota.ObterPorFechamentosTurma(fechamentosIds);

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

        private async Task<bool> PodeEditarNotaOuConceitoPeriodoUsuario(Usuario usuarioLogado, PeriodoEscolar periodoEscolar, Turma turma, string codigoComponenteCurricular, DateTime data)
        {
            if (!usuarioLogado.EhGestorEscolar() && !usuarioLogado.EhPerfilDRE() && !usuarioLogado.EhPerfilSME())
            {
                var usuarioPodeEditar = await mediator.Send(new PodePersistirTurmaDisciplinaQuery(usuarioLogado.CodigoRf, turma.CodigoTurma, codigoComponenteCurricular, data.Ticks));
                if (!usuarioPodeEditar)
                    return false;
            }

            var temPeriodoAberto = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma, DateTimeExtension.HorarioBrasilia().Date, periodoEscolar.Bimestre);

            return temPeriodoAberto;
        }

        private async Task<FechamentoFinalConsultaRetornoAlunoDto> TrataFrequenciaAluno(FechamentoFinalConsultaFiltroDto filtros, IEnumerable<PeriodoEscolar> periodosEscolares, AlunoPorTurmaResposta aluno, Turma turma)
        {
            var frequenciaAluno = await mediator.Send(new ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery(aluno.CodigoAluno, turma.CodigoTurma, filtros.DisciplinaCodigo.ToString()));

            var existeFrequenciaComponenteCurricular = await repositorioFrequenciaAlunoDisciplinaPeriodo.ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(turma.CodigoTurma,
                filtros.DisciplinaCodigo.ToString(), periodosEscolares.Select(c => c.Id).ToArray());

            var percentualFrequencia = frequenciaAluno?.PercentualFrequencia ?? 0;

            if (frequenciaAluno != null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto
            {
                Nome = aluno.NomeAluno,
                TotalAusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = existeFrequenciaComponenteCurricular ? percentualFrequencia.ToString() : null,
                TotalFaltas = frequenciaAluno?.TotalAusencias ?? 0,
                NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
            };
            return fechamentoFinalAluno;
        }

        private void TrataPeriodosEscolaresParaAluno(FechamentoFinalConsultaFiltroDto filtros, AlunoPorTurmaResposta aluno, ref int totalAusencias, ref int totalAusenciasCompensadas, ref int totalDeAulas, PeriodoEscolar periodo)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(aluno.CodigoAluno, periodo.PeriodoFim, TipoFrequenciaAluno.PorDisciplina, filtros.DisciplinaCodigo.ToString());
            if (frequenciaAluno != null)
            {
                totalAusencias += frequenciaAluno.TotalAusencias;
                totalAusenciasCompensadas += frequenciaAluno.TotalCompensacoes;
                totalDeAulas += frequenciaAluno.TotalAulas;
            }
        }

        private async Task VerificaSePodeFazerFechamentoFinal(IEnumerable<PeriodoEscolar> periodosEscolares, Turma turma)
        {
            var ultimoBimestre = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().Bimestre;

            var fechamentoDoUltimoBimestre = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, null, ultimoBimestre);

            if (fechamentoDoUltimoBimestre == null || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoBimestre}º  bimestre.");
        }
    }
}