using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IConsultasAulaPrevista consultasAulaPrevista;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasFechamentoFinal(IConsultasAulaPrevista consultasAulaPrevista, IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario,
                            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                            IServicoEol servicoEOL, IRepositorioFechamentoNota repositorioFechamentoNota,
                            IServicoAluno servicoAluno,
                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioNotaTipoValor repositorioNotaTipoValor,
                            IServicoUsuario servicoUsuario, IRepositorioParametrosSistema repositorioParametrosSistema,
                            IConsultasDisciplina consultasDisciplina, IConsultasFrequencia consultasFrequencia, IConsultasPeriodoFechamento consultasPeriodoFechamento,
                            IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                            IMediator mediator)
        {
            this.consultasAulaPrevista = consultasAulaPrevista ?? throw new System.ArgumentNullException(nameof(consultasAulaPrevista));
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
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFrequencia = consultasFrequencia ?? throw new System.ArgumentNullException(nameof(consultasFrequencia));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(filtros.TurmaCodigo);

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), filtros.semestre);
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            if (turma.AnoLetivo != 2020)
                await VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma);

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();

            retorno.EventoData = ultimoPeriodoEscolar.PeriodoFim;

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            var turmaEOL = await servicoEOL.ObterDadosTurmaPorCodigo(turma.CodigoTurma);
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
                var disciplinasRegencia = await consultasDisciplina.ObterComponentesRegencia(turma, filtros.DisciplinaCodigo);

                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaResposta() { Nome = disciplinaEOL.Nome, CodigoComponenteCurricular = disciplinaEOL.CodigoComponenteCurricular });

            retorno.EhSintese = !disciplinaEOL.LancaNota;

            var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.CodigoTurma, new long[] { filtros.DisciplinaCodigo });
            var notasFechamentosFinais = Enumerable.Empty<FechamentoNota>();
            if (fechamentosTurmaDisciplina != null && fechamentosTurmaDisciplina.Any())
                notasFechamentosFinais = await repositorioFechamentoNota.ObterPorFechamentosTurma(fechamentosTurmaDisciplina.Select(ftd => ftd.Id).ToArray());

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
                    var sinteseDto = await consultasFrequencia.ObterSinteseAluno(fechamentoFinalAluno.FrequenciaValor, disciplinaEOL);
                    fechamentoFinalAluno.Sintese = sinteseDto.Valor;
                }
                else
                {
                    foreach (var periodo in periodosEscolares.OrderBy(a => a.Bimestre))
                    {
                        foreach (var disciplinaParaAdicionar in disciplinas)
                        {
                            //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
                            var nota = notasFechamentosBimestres.FirstOrDefault(a => a.Bimestre == periodo.Bimestre && a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular && a.AlunoCodigo == aluno.CodigoAluno);
                            var notaParaAdicionar = (nota == null ? null : nota.NotaConceito);

                            fechamentoFinalAluno.NotasConceitoBimestre.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                            {
                                Bimestre = periodo.Bimestre,
                                Disciplina = disciplinaParaAdicionar.Nome,
                                DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                                NotaConceito = notaParaAdicionar,
                                
                            });
                        }
                    }

                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        var nota = notasFechamentosFinais.FirstOrDefault(a => a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular && a.FechamentoAluno.AlunoCodigo == aluno.CodigoAluno);
                        string notaParaAdicionar = string.Empty;
                        if (nota != null)
                            notaParaAdicionar = (tipoNota.EhNota() ? nota.Nota.ToString() : nota.ConceitoId.ToString());

                        fechamentoFinalAluno.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Disciplina = disciplinaParaAdicionar.Nome,
                            DisciplinaCodigo = disciplinaParaAdicionar.CodigoComponenteCurricular,
                            NotaConceito = notaParaAdicionar == string.Empty ? null : notaParaAdicionar,
                        });
                    }
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.PodeEditarNotaConceito() : false;
                fechamentoFinalAluno.Codigo = aluno.CodigoAluno;
                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            retorno.AuditoriaAlteracao = MontaTextoAuditoriaAlteracao(fechamentosTurmaDisciplina.Any() ? fechamentosTurmaDisciplina.FirstOrDefault() : null, retorno.EhNota);
            retorno.AuditoriaInclusao = MontaTextoAuditoriaInclusao(fechamentosTurmaDisciplina.Any() ? fechamentosTurmaDisciplina.FirstOrDefault() : null, retorno.EhNota);

            retorno.NotaMedia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, DateTime.Today.Year)));
            retorno.FrequenciaMedia = await consultasFrequencia.ObterFrequenciaMedia(disciplinaEOL);

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

            //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
            foreach (var periodo in periodosEscolares)
            {
                var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.CodigoTurma, new long[] { disciplinaCodigo }, periodo.Bimestre);

                foreach (var fechamentoTurmaDisciplina in fechamentosTurmaDisciplina)
                {
                    var notasDoBimestre = await repositorioFechamentoNota.ObterPorFechamentoTurma(fechamentoTurmaDisciplina.Id);
                    if (notasDoBimestre != null && notasDoBimestre.Any())
                    {
                        foreach (var nota in notasDoBimestre)
                        {
                            var notaParaAdicionar = ehNota ? nota.Nota?.ToString() : nota.ConceitoId?.ToString();
                            listaRetorno.Add(new FechamentoNotaAlunoDto(periodo.Bimestre, notaParaAdicionar, nota.DisciplinaId, nota.FechamentoAluno.AlunoCodigo));
                        }
                    }
                }
            }

            return listaRetorno;
        }

        private async Task<bool> PodeEditarNotaOuConceitoPeriodoUsuario(Usuario usuarioLogado, PeriodoEscolar periodoEscolar, Turma turma, string codigoComponenteCurricular, DateTime data)
        {
            if (!usuarioLogado.EhGestorEscolar())
            {
                var usuarioPodeEditar = await servicoEOL.PodePersistirTurmaDisciplina(usuarioLogado.CodigoRf, turma.CodigoTurma, codigoComponenteCurricular, data);
                if (!usuarioPodeEditar)
                    return false;
            }               

            var periodoFechamento = await consultasPeriodoFechamento.ObterPeriodoFechamentoTurmaAsync(turma, periodoEscolar.Bimestre, periodoEscolar.Id);

            var reabertura = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(periodoEscolar.Bimestre, DateTime.Today, periodoEscolar.TipoCalendarioId);

            var dentroPeriodo = (periodoFechamento != null && periodoFechamento.DataDentroPeriodo(DateTime.Today)) ||
                (reabertura != null && reabertura.DataDentroPeriodo(DateTime.Now));

            return dentroPeriodo;
        }

        private async Task<FechamentoFinalConsultaRetornoAlunoDto> TrataFrequenciaAluno(FechamentoFinalConsultaFiltroDto filtros, IEnumerable<PeriodoEscolar> periodosEscolares, AlunoPorTurmaResposta aluno, Turma turma)
        {
            var frequenciaAluno = await consultasFrequencia.ObterFrequenciaGeralAlunoPorTurmaEComponente(aluno.CodigoAluno, turma.CodigoTurma, filtros.DisciplinaCodigo.ToString());

            var percentualFrequencia = frequenciaAluno?.PercentualFrequencia ?? 100;

            if (frequenciaAluno != null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto
            {
                Nome = aluno.NomeAluno,
                TotalAusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia.ToString(),
                TotalFaltas = frequenciaAluno?.TotalAusencias ?? 0,
                NumeroChamada = aluno.NumeroAlunoChamada,
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