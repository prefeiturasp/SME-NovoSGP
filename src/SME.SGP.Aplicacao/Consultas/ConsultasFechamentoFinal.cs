using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoFinal : IConsultasFechamentoFinal
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioNotaTipoValor repositorioNotaTipoValor;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFrequencia consultasFrequencia;

        public ConsultasFechamentoFinal(IRepositorioTurma repositorioTurma, IRepositorioTipoCalendario repositorioTipoCalendario,
                            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IServicoEOL servicoEOL, IRepositorioFechamentoNota repositorioFechamentoNota,
            IServicoAluno servicoAluno,
            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo, IRepositorioNotaTipoValor repositorioNotaTipoValor,
            IServicoUsuario servicoUsuario, IRepositorioParametrosSistema repositorioParametrosSistema,
            IConsultasDisciplina consultasDisciplina, IConsultasFrequencia consultasFrequencia)
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
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametrosSistema));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentNullException(nameof(consultasDisciplina));
            this.consultasFrequencia = consultasFrequencia ?? throw new System.ArgumentNullException(nameof(consultasFrequencia));
        }

        public async Task<FechamentoFinalConsultaRetornoDto> ObterFechamentos(FechamentoFinalConsultaFiltroDto filtros)
        {
            var retorno = new FechamentoFinalConsultaRetornoDto();
            var turma = repositorioTurma.ObterPorCodigo(filtros.TurmaCodigo);

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario());
            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            await VerificaSePodeFazerFechamentoFinal(periodosEscolares, turma);

            retorno.EventoData = periodosEscolares.OrderByDescending(a => a.Bimestre).FirstOrDefault().PeriodoFim;

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
                throw new NegocioException("Não foram encontrandos alunos para a turma informada.");

            var tipoNota = repositorioNotaTipoValor.ObterPorTurmaId(turma.Id);
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
                var disciplinasRegencia = await servicoEOL.ObterDisciplinasParaPlanejamento(long.Parse(turma.CodigoTurma), usuarioAtual.Login, usuarioAtual.PerfilAtual);
                if (disciplinasRegencia == null || !disciplinasRegencia.Any())
                    throw new NegocioException("Não foi encontrado componentes curriculares para a regencia informada.");

                disciplinas.AddRange(disciplinasRegencia);
            }
            else
                disciplinas.Add(new DisciplinaResposta() { Nome = disciplinaEOL.Nome, CodigoComponenteCurricular = disciplinaEOL.CodigoComponenteCurricular });

            retorno.EhSintese = !disciplinaEOL.LancaNota;

            var fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turma.CodigoTurma, filtros.DisciplinaCodigo);
            var notasFechamentosFinais = Enumerable.Empty<FechamentoNota>();
            if (fechamentoTurmaDisciplina != null)
            {
                notasFechamentosFinais = await repositorioFechamentoNota.ObterPorFechamentoTurma(fechamentoTurmaDisciplina.Id);
            }

            var notasFechamentosBimestres = Enumerable.Empty<FechamentoNotaAlunoDto>();
            if (!retorno.EhSintese)
            {
               notasFechamentosBimestres = await ObterNotasFechamentosBimestres(filtros.DisciplinaCodigo, turma, periodosEscolares, retorno.EhNota);
            }             

            var professorTitular = await ObterRfProfessorTitularDisciplina(turma.CodigoTurma, filtros.DisciplinaCodigo);
            if (string.IsNullOrEmpty(professorTitular))
                throw new NegocioException("Não foi possível localizar o professor titular.");
            foreach (var aluno in alunosDaTurma.Where(a => a.NumeroAlunoChamada > 0 || a.CodigoSituacaoMatricula.Equals(SituacaoMatriculaAluno.Ativo)).OrderBy(a => a.NumeroAlunoChamada).ThenBy(a => a.NomeValido()))
            {
                var totalAusencias = 0;
                var totalAusenciasCompensadas = 0;
                var totalDeAulas = 0;

                FechamentoFinalConsultaRetornoAlunoDto fechamentoFinalAluno = TrataFrequenciaAluno(filtros, periodosEscolares, aluno, ref totalAusencias, ref totalAusenciasCompensadas, ref totalDeAulas);

                var marcador = servicoAluno.ObterMarcadorAluno(aluno, new PeriodoEscolarDto() { PeriodoFim = retorno.EventoData });
                if (marcador != null)
                    fechamentoFinalAluno.Informacao = marcador.Descricao;

                if (retorno.EhSintese)
                {
                    var sinteseDto = consultasFrequencia.ObterSinteseAluno(fechamentoFinalAluno.Frequencia, disciplinaEOL);
                    fechamentoFinalAluno.Sintese = sinteseDto.SinteseNome;
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
                                NotaConceito = notaParaAdicionar
                            });
                        }
                    }

                    foreach (var disciplinaParaAdicionar in disciplinas)
                    {
                        var nota = notasFechamentosFinais.FirstOrDefault(a => a.DisciplinaId == disciplinaParaAdicionar.CodigoComponenteCurricular && a.FechamentoAluno.AlunoCodigo == aluno.CodigoAluno); ;
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

                fechamentoFinalAluno.PodeEditar = PodeEditarNotaOuConceito(usuarioAtual, professorTitular, aluno);
                fechamentoFinalAluno.Codigo = aluno.CodigoAluno;
                retorno.Alunos.Add(fechamentoFinalAluno);
            }

            retorno.AuditoriaAlteracao = MontaTextoAuditoriaAlteracao(fechamentoTurmaDisciplina, retorno.EhNota);
            retorno.AuditoriaInclusao = MontaTextoAuditoriaInclusao(fechamentoTurmaDisciplina, retorno.EhNota);

            retorno.NotaMedia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.MediaBimestre));
            retorno.FrequenciaMedia = consultasFrequencia.ObterFrequenciaMedia(disciplinaEOL);

            return retorno;
        }

        private static bool PodeEditarNotaOuConceito(Usuario usuarioLogado, string professorTitularDaTurmaDisciplinaRf, AlunoPorTurmaResposta aluno)
        {
            if (aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.Ativo &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.PendenteRematricula &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.Rematriculado &&
                aluno.CodigoSituacaoMatricula != SituacaoMatriculaAluno.SemContinuidade)
                return false;

            if (usuarioLogado.CodigoRf != professorTitularDaTurmaDisciplinaRf)
                return false;

            return true;
        }

        private string MontaTextoAuditoriaAlteracao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            if (fechamentoTurmaDisciplina != null && !string.IsNullOrEmpty(fechamentoTurmaDisciplina.AlteradoPor))
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "alteradas" : "alterados")} por {fechamentoTurmaDisciplina.AlteradoPor}({fechamentoTurmaDisciplina.AlteradoRF}) em {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.AlteradoEm.Value.ToString("H:mm")}.";
            else return string.Empty;
        }

        private string MontaTextoAuditoriaInclusao(FechamentoTurmaDisciplina fechamentoTurmaDisciplina, bool EhNota)
        {
            if (fechamentoTurmaDisciplina != null)
                return $"{(EhNota ? "Notas" : "Conceitos")} finais {(EhNota ? "incluídas" : "incluídos")} por {fechamentoTurmaDisciplina.CriadoPor}({fechamentoTurmaDisciplina.CriadoRF}) em {fechamentoTurmaDisciplina.CriadoEm.ToString("dd/MM/yyyy")},às {fechamentoTurmaDisciplina.CriadoEm.ToString("H:mm")}.";
            else return string.Empty;
        }

        private async Task<IEnumerable<FechamentoNotaAlunoDto>> ObterNotasFechamentosBimestres(long disciplinaCodigo, Turma turma, IEnumerable<PeriodoEscolar> periodosEscolares, bool ehNota)
        {
            var listaRetorno = new List<FechamentoNotaAlunoDto>();

            //BIMESTRE / NOTA / DISCIPLINA ID / ALUNO CODIGO
            foreach (var periodo in periodosEscolares)
            {
                var fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(turma.CodigoTurma, disciplinaCodigo, periodo.Bimestre);

                if (fechamentoTurmaDisciplina != null)
                {
                    var notasDoBimestre = await repositorioFechamentoNota.ObterPorFechamentoTurma(fechamentoTurmaDisciplina.Id);
                    if (notasDoBimestre != null && notasDoBimestre.Any())
                    {
                        foreach (var nota in notasDoBimestre)
                        {
                            var notaParaAdicionar = ehNota ? nota.Nota.ToString() : nota.ConceitoId.Value.ToString();
                            listaRetorno.Add(new FechamentoNotaAlunoDto(periodo.Bimestre, notaParaAdicionar, nota.DisciplinaId, nota.FechamentoAluno.AlunoCodigo));
                        }
                    }
                }
            }

            return listaRetorno;
        }

        private async Task<string> ObterRfProfessorTitularDisciplina(string turmaCodigo, long disciplinaCodigo)
        {
            var professoresTitularesDaTurma = await servicoEOL.ObterProfessoresTitularesDisciplinas(turmaCodigo);
            var professorTitularDaDisciplina = professoresTitularesDaTurma.FirstOrDefault(a => a.DisciplinaId == disciplinaCodigo && a.ProfessorRf != string.Empty);
            return professorTitularDaDisciplina == null ? string.Empty : professorTitularDaDisciplina.ProfessorRf;
        }

        private FechamentoFinalConsultaRetornoAlunoDto TrataFrequenciaAluno(FechamentoFinalConsultaFiltroDto filtros, IEnumerable<PeriodoEscolar> periodosEscolares, AlunoPorTurmaResposta aluno, ref int totalAusencias, ref int totalAusenciasCompensadas, ref int totalDeAulas)
        {
            foreach (var periodo in periodosEscolares)
            {
                TrataPeriodosEscolaresParaAluno(filtros, aluno, ref totalAusencias, ref totalAusenciasCompensadas, ref totalDeAulas, periodo);
            }
            var percentualFrequencia = 100;
            if (totalDeAulas != 0)
                percentualFrequencia = (((totalDeAulas - totalAusencias) + totalAusenciasCompensadas) / totalDeAulas) * 100;

            var fechamentoFinalAluno = new FechamentoFinalConsultaRetornoAlunoDto
            {
                Nome = aluno.NomeAluno,
                TotalAusenciasCompensadas = totalAusenciasCompensadas,
                Frequencia = percentualFrequencia,
                TotalFaltas = totalAusencias,
                NumeroChamada = aluno.NumeroAlunoChamada
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