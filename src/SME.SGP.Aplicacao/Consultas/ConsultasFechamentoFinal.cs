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
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;
        private const long ID_COMPONENTE_ED_FISICA = 6;

        public ConsultasFechamentoFinal(IRepositorioTurmaConsulta repositorioTurma, IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
                            IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
                            IServicoAluno servicoAluno,
                            IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo,
                            IServicoUsuario servicoUsuario,
                            IConsultasDisciplina consultasDisciplina, IConsultasPeriodoFechamento consultasPeriodoFechamento,
                            IMediator mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoNota));
            this.servicoAluno = servicoAluno ?? throw new System.ArgumentNullException(nameof(servicoAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentNullException(nameof(consultasDisciplina));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(filtros.TurmaCodigo);

            if (turma.EhNulo())
                throw new NegocioException("Não foi possível localizar a turma.");

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeCodigo.ObterModalidadeTipoCalendario(), filtros.Semestre);
            if (tipoCalendario.EhNulo())
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            if (turma.AnoLetivo != 2020)
                await VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma, tipoCalendario.Id);

            var ultimoPeriodoEscolar = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault();

            retorno.EventoData = ultimoPeriodoEscolar.PeriodoInicio;

            var fechamentoDoUltimoBimestre = await mediator.Send(new ObterFechamentosTurmaComponentesQuery(turma.Id, new long[] { filtros.DisciplinaCodigo }, ultimoPeriodoEscolar.Bimestre, tipoCalendario.Id));

            if (fechamentoDoUltimoBimestre.EhNulo() || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoPeriodoEscolar.Bimestre}º  bimestre.");
            
            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma, turma.AnoLetivo));
            
            if (alunosDaTurma.EhNulo() || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            var turmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(turma.CodigoTurma));
            
            var tipoNota = await mediator.Send(new ObterNotaTipoValorPorTurmaIdQuery(turma));
            if (tipoNota.EhNulo())
                throw new NegocioException("Não foi possível localizar o tipo de nota para esta turma.");

            retorno.EhNota = tipoNota.EhNota();
            //Codigo aluno / NotaConceito / Código Disciplina / bimestre

            var disciplinas = new List<DisciplinaResposta>();
            var disciplinaEOL = await consultasDisciplina.ObterDisciplina(filtros.DisciplinaCodigo);
            var usuarioAtual = await servicoUsuario.ObterUsuarioLogado();

            var periodoFechamentoFinal = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(tipoCalendario.Id, turma.EhTurmaInfantil, turma.EhTurmaModalidadeSemestral() ? 2 : 4));

            if(periodoFechamentoFinal.EhNulo())
                throw new NegocioException("Não foi possível localizar o período de fechamento final para essa turma.");

            var datasFechamentoFinal = new PeriodoEscolar()
            {
                PeriodoInicio = periodoFechamentoFinal.InicioDoFechamento,
                PeriodoFim = periodoFechamentoFinal.FinalDoFechamento
            };

            if (filtros.EhRegencia)
            {
                var disciplinasRegencia = await mediator.Send(new ObterComponentesRegenciaPorAnoQuery(turma.TipoTurno == 4 || turma.TipoTurno == 5 ? turma.AnoTurmaInteiro : 0));

                if (disciplinasRegencia.EhNulo() || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

                if (turma.EhEJA())
                    disciplinasRegencia = disciplinasRegencia.Where(a => a.CodigoComponenteCurricular != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaResposta() { Nome = disciplinaEOL.Nome, CodigoComponenteCurricular = disciplinaEOL.CodigoComponenteCurricular });

            if (turma.TipoTurma != TipoTurma.EdFisica && turma.ModalidadeCodigo == Modalidade.EJA && disciplinas.Any(d => d.CodigoComponenteCurricular == ID_COMPONENTE_ED_FISICA))
                disciplinas.Remove(disciplinas.FirstOrDefault(d => d.CodigoComponenteCurricular == ID_COMPONENTE_ED_FISICA));

            retorno.EhSintese = !disciplinaEOL.LancaNota;

            var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, new long[] { filtros.DisciplinaCodigo });
            var notasFechamentosFinais = Enumerable.Empty<FechamentoNotaAlunoAprovacaoDto>();

            if (fechamentosTurmaDisciplina.NaoEhNulo() && fechamentosTurmaDisciplina.Any())
                notasFechamentosFinais = await mediator.Send(new ObterPorFechamentosTurmaQuery(fechamentosTurmaDisciplina.Select(ftd => ftd.Id).ToArray(), turma.CodigoTurma, filtros.DisciplinaCodigo.ToString()));

            var notasFechamentosBimestres = Enumerable.Empty<FechamentoNotaAlunoDto>();

            if (!retorno.EhSintese)
                notasFechamentosBimestres = await ObterNotasFechamentosBimestres(filtros.DisciplinaCodigo, turma, retorno.EhNota, tipoCalendario.Id);


            var usuarioEPeriodoPodeEditar = await PodeEditarNotaOuConceitoPeriodoUsuario(usuarioAtual, ultimoPeriodoEscolar, turma, filtros.DisciplinaCodigo.ToString(), retorno.EventoData);
            var alunosValidos = alunosDaTurma
                .Where(a => a.NumeroAlunoChamada > 0 ||
                            a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo) ||
                            a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Concluido));

            var alunosValidosOrdenados = alunosValidos.Where(a => a.EstaAtivo(ultimoPeriodoEscolar.PeriodoInicio, ultimoPeriodoEscolar.PeriodoFim)).OrderBy(a => a.NomeAluno).ThenBy(a => a.NomeValido());

            foreach (var aluno in alunosValidosOrdenados)
            {
                FechamentoFinalConsultaRetornoAlunoDto fechamentoFinalAluno = await TrataFrequenciaAluno(filtros, periodosEscolares, aluno, turma);

                var marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolar() { PeriodoFim = retorno.EventoData });
                if (marcador.NaoEhNulo())
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
                                                                        && a.AlunoCodigo == aluno.CodigoAluno && (a.Bimestre is null || a.Bimestre == (int)Bimestre.Final));
                        string notaParaAdicionar = string.Empty;

                        if (nota.NaoEhNulo())
                        {
                            if (tipoNota.EhNota())
                                notaParaAdicionar = nota.Nota.HasValue ? nota.Nota.Value.ToString() : string.Empty;
                            else
                                notaParaAdicionar = nota.ConceitoId.HasValue ? nota.ConceitoId.Value.ToString() : string.Empty;
                        }

                        fechamentoFinalAluno.NotasConceitoFinal.Add(new FechamentoFinalConsultaRetornoAlunoNotaConceitoDto()
                        {
                            Disciplina = disciplina.Nome,
                            DisciplinaCodigo = disciplina.CodigoComponenteCurricular,
                            NotaConceito = notaParaAdicionar,
                            EmAprovacao = nota?.EmAprovacao ?? false
                        });

                    }
                }

                fechamentoFinalAluno.PodeEditar = usuarioEPeriodoPodeEditar ? aluno.VerificaSePodeEditarAluno(datasFechamentoFinal) : false;
                fechamentoFinalAluno.Codigo = aluno.CodigoAluno;
                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            retorno.AuditoriaAlteracao = AuditoriaUtil.MontarTextoAuditoriaAlteracao(fechamentosTurmaDisciplina.FirstOrDefault(), retorno.EhNota);
            retorno.AuditoriaInclusao = AuditoriaUtil.MontarTextoAuditoriaInclusao(fechamentosTurmaDisciplina.FirstOrDefault(), retorno.EhNota);

            retorno.NotaMedia = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.MediaBimestre, turma.AnoLetivo)));
            retorno.FrequenciaMedia = await mediator.Send(new ObterFrequenciaMediaQuery(disciplinaEOL, turma.AnoLetivo));
            retorno.PeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, ultimoPeriodoEscolar.Bimestre, turma.AnoLetivo == DateTime.Today.Year));
            retorno.DadosArredondamento = await mediator.Send(new ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery(ultimoPeriodoEscolar.PeriodoFim));
            return retorno;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, bool ehNota,long? tipoCalendario = null)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();
            var fechamentosTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, new long[] { disciplinaCodigo }, -1, tipoCalendario);
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
               new string[] { filtros.DisciplinaCodigo.ToString() }, periodosEscolares.Select(c => c.Id).ToArray());

            var percentualFrequencia = frequenciaAluno?.PercentualFrequencia;

            if (frequenciaAluno.NaoEhNulo() && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var percentualFrequenciaFormatado = percentualFrequencia.HasValue ? FrequenciaAluno.FormatarPercentual(percentualFrequencia.GetValueOrDefault()) : string.Empty;

            var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto
            {
                Nome = aluno.NomeAluno,
                TotalAusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = existeFrequenciaComponenteCurricular ? percentualFrequenciaFormatado : null,
                TotalFaltas = frequenciaAluno?.TotalAusencias ?? 0,
                NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo))
            };
            return fechamentoFinalAluno;
        }

        private async Task VerificaSePodeFazerFechamentoFinal(IEnumerable<PeriodoEscolar> periodosEscolares, Turma turma, long? tipoCalendario = null)
        {
            var ultimoBimestre = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().Bimestre;

            var fechamentoDoUltimoBimestre = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(turma.Id, null, ultimoBimestre, tipoCalendario);

            if (fechamentoDoUltimoBimestre.EhNulo() || !fechamentoDoUltimoBimestre.Any())
                throw new NegocioException($"Para acessar este aba você precisa realizar o fechamento do {ultimoBimestre}º  bimestre.");
        }
    }
}