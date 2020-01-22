using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFrequencia : IConsultasFrequencia
    {
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;

        public ConsultasFrequencia(IServicoFrequencia servicoFrequencia,
                                   IServicoEOL servicoEOL,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioFrequencia repositorioFrequencia,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                   IRepositorioParametrosSistema repositorioParametrosSistema, IServicoAluno servicoAluno)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
            => await repositorioFrequencia.FrequenciaAulaRegistrada(aulaId);

        public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(aula.TurmaId, aula.DataAula.Year);
            if (alunosDaTurma == null || !alunosDaTurma.Any())
            {
                throw new NegocioException("Não foram encontrados alunos para a aula/turma informada.");
            }
            var turma = repositorioTurma.ObterPorId(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");
            FrequenciaDto registroFrequenciaDto = ObterRegistroFrequencia(aulaId, aula, turma);

            var ausencias = servicoFrequencia.ObterListaAusenciasPorAula(aulaId);
            if (ausencias == null)
            {
                ausencias = new List<RegistroAusenciaAluno>();
            }

            var bimestre = consultasPeriodoEscolar.ObterPeriodoEscolarPorData(aula.TipoCalendarioId, aula.DataAula);
            var percentualCritico = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.PercentualFrequenciaCritico,
                                                    bimestre.PeriodoInicio.Year));
            var percentualAlerta = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.PercentualFrequenciaAlerta,
                                                    bimestre.PeriodoInicio.Year));

            var disciplinaAula = servicoEOL.ObterDisciplinasPorIds(new long[] { Convert.ToInt64(aula.DisciplinaId) });

            if (disciplinaAula == null || disciplinaAula.ToList().Count <= 0)
                throw new NegocioException("Disciplina da aula não encontrada");

            foreach (var aluno in alunosDaTurma.Where(a => a.DeveMostrarNaChamada()))
            {
                // Apos o bimestre da inatividade o aluno não aparece mais na lista de frequencia
                if (aluno.EstaInativo() && (aluno.DataSituacao < bimestre.PeriodoInicio))
                    continue;

                var registroFrequenciaAluno = new RegistroFrequenciaAlunoDto
                {
                    CodigoAluno = aluno.CodigoAluno,
                    NomeAluno = aluno.NomeAluno,
                    NumeroAlunoChamada = aluno.NumeroAlunoChamada,
                    CodigoSituacaoMatricula = aluno.CodigoSituacaoMatricula,
                    SituacaoMatricula = aluno.SituacaoMatricula,
                    Desabilitado = aluno.EstaInativo() && (aula.DataAula.Date >= aluno.DataSituacao.Date)
                };

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, bimestre);

                // Indicativo de frequencia do aluno
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(aluno, aula.DisciplinaId, bimestre, percentualAlerta, percentualCritico);

                if (disciplinaAula.FirstOrDefault().RegistroFrequencia)
                {
                    var ausenciasAluno = ausencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);

                    for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                    {
                        registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                        {
                            NumeroAula = numeroAula,
                            Compareceu = !ausenciasAluno.Any(c => c.NumeroAula == numeroAula)
                        });
                    }
                }
                registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
            }
            if (registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado))
            {
                registroFrequenciaDto.Desabilitado = true;
            }
            return registroFrequenciaDto;
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(AlunoPorTurmaResposta aluno, string disciplinaId, PeriodoEscolarDto bimestre, int percentualAlerta, int percentualCritico)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(aluno.CodigoAluno, disciplinaId, bimestre.PeriodoInicio, bimestre.PeriodoFim, TipoFrequenciaAluno.PorDisciplina);
            // Frequencia não calculada
            if (frequenciaAluno == null)
                return null;

            var percentualFrequencia = (int)(100 - (frequenciaAluno.TotalAusencias / frequenciaAluno.TotalAulas * 100));

            // Critico
            if (percentualFrequencia <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequencia };

            // Alerta
            if (percentualFrequencia <= percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequencia };

            return null;
        }

        private FrequenciaDto ObterRegistroFrequencia(long aulaId, Aula aula, Turma turma)
        {
            var registroFrequencia = servicoFrequencia.ObterRegistroFrequenciaPorAulaId(aulaId);
            if (registroFrequencia == null)
            {
                registroFrequencia = new RegistroFrequencia(aula);
            }
            var registroFrequenciaDto = new FrequenciaDto(aulaId)
            {
                AlteradoEm = registroFrequencia.AlteradoEm,
                AlteradoPor = registroFrequencia.AlteradoPor,
                AlteradoRF = registroFrequencia.AlteradoRF,
                CriadoEm = registroFrequencia.CriadoEm,
                CriadoPor = registroFrequencia.CriadoPor,
                CriadoRF = registroFrequencia.CriadoRF,
                Id = registroFrequencia.Id,
                Desabilitado = !aula.PermiteRegistroFrequencia(turma)
            };
            return registroFrequenciaDto;
        }
    }
}