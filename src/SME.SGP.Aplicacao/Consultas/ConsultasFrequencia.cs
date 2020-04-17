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
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoAluno servicoAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;

        private double _mediaFrequencia;

        public ConsultasFrequencia(IServicoFrequencia servicoFrequencia,
                                   IServicoEOL servicoEOL,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                   IConsultasTipoCalendario consultasTipoCalendario,
                                   IConsultasTurma consultasTurma,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioFrequencia repositorioFrequencia,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                   IRepositorioParametrosSistema repositorioParametrosSistema,
                                   IServicoAluno servicoAluno)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
            => await repositorioFrequencia.FrequenciaAulaRegistrada(aulaId);

        public async Task<IEnumerable<AlunoAusenteDto>> ObterListaAlunosComAusencia(string turmaId, string disciplinaId, int bimestre)
        {
            var alunosAusentesDto = new List<AlunoAusenteDto>();
            // Busca dados da turma
            var turma = BuscaTurma(turmaId);

            // Busca periodo
            var periodo = BuscaPeriodo(turma.AnoLetivo, turma.ModalidadeCodigo, bimestre, turma.Semestre);

            var alunosEOL = await servicoEOL.ObterAlunosPorTurma(turmaId);
            if (alunosEOL == null || !alunosEOL.Any())
                throw new NegocioException("Não foram localizados alunos para a turma selecionada.");

            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(disciplinaId) });
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Disciplina informada não localizada no EOL.");

            var quantidadeMaximaCompensacoes = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.QuantidadeMaximaCompensacaoAusencia));
            var percentualFrequenciaAlerta = int.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(disciplinasEOL.First().Regencia ? TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse : TipoParametroSistema.CompensacaoAusenciaPercentualFund2));

            foreach (var alunoEOL in alunosEOL)
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(alunoEOL.CodigoAluno, disciplinaId, periodo.PeriodoFim);
                if (frequenciaAluno == null || frequenciaAluno.NumeroFaltasNaoCompensadas == 0)
                    continue;

                var faltasNaoCompensadas = int.Parse(frequenciaAluno.NumeroFaltasNaoCompensadas.ToString());

                alunosAusentesDto.Add(new AlunoAusenteDto()
                {
                    Id = alunoEOL.CodigoAluno,
                    Nome = alunoEOL.NomeAluno,
                    QuantidadeFaltasTotais = faltasNaoCompensadas,
                    MaximoCompensacoesPermitidas = quantidadeMaximaCompensacoes > faltasNaoCompensadas ? faltasNaoCompensadas : quantidadeMaximaCompensacoes,
                    PercentualFrequencia = frequenciaAluno.PercentualFrequencia,
                    Alerta = frequenciaAluno.PercentualFrequencia <= percentualFrequenciaAlerta
                });
            }

            return alunosAusentesDto;
        }

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
            var turma = repositorioTurma.ObterPorCodigo(aula.TurmaId);
            if (turma == null)
                throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");
            FrequenciaDto registroFrequenciaDto = ObterRegistroFrequencia(aulaId, aula, turma);

            var ausencias = servicoFrequencia.ObterListaAusenciasPorAula(aulaId);
            if (ausencias == null)
            {
                ausencias = new List<RegistroAusenciaAluno>();
            }

            var bimestre = consultasPeriodoEscolar.ObterPeriodoEscolarPorData(aula.TipoCalendarioId, aula.DataAula);
            if (bimestre == null)
            {
                throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");
            }

            registroFrequenciaDto.TemPeriodoAberto = await consultasTurma.TurmaEmPeriodoAberto(aula.TurmaId, DateTime.Today, bimestre.Bimestre);

            var parametroPercentualCritico = repositorioParametrosSistema.ObterValorPorTipoEAno(
                                                    TipoParametroSistema.PercentualFrequenciaCritico,
                                                    bimestre.PeriodoInicio.Year);
            if (parametroPercentualCritico == null)
            {
                throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");
            }
            var percentualCritico = int.Parse(parametroPercentualCritico);
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
                    Desabilitado = (aluno.EstaInativo() && (aula.DataAula.Date >= aluno.DataSituacao.Date)) || aula.EhDataSelecionadaFutura,
                };

                // Marcador visual da situação
                registroFrequenciaAluno.Marcador = servicoAluno.ObterMarcadorAluno(aluno, bimestre);

                // Indicativo de frequencia do aluno
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(aluno, aula.DisciplinaId, bimestre, percentualAlerta, percentualCritico);

                if (!disciplinaAula.FirstOrDefault().RegistraFrequencia)
                {
                    registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
                    continue;
                }

                var ausenciasAluno = ausencias.Where(c => c.CodigoAluno == aluno.CodigoAluno);

                for (int numeroAula = 1; numeroAula <= aula.Quantidade; numeroAula++)
                {
                    registroFrequenciaAluno.Aulas.Add(new FrequenciaAulaDto
                    {
                        NumeroAula = numeroAula,
                        Compareceu = !ausenciasAluno.Any(c => c.NumeroAula == numeroAula)
                    });
                }

                registroFrequenciaDto.ListaFrequencia.Add(registroFrequenciaAluno);
            }

            registroFrequenciaDto.Desabilitado = registroFrequenciaDto.ListaFrequencia.All(c => c.Desabilitado) || aula.EhDataSelecionadaFutura;

            return registroFrequenciaDto;
        }

        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual)
            => repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoDisciplinaData(codigoAluno, disciplinaId, dataAtual);

        public SinteseDto ObterSinteseAluno(double percentualFrequencia, DisciplinaDto disciplina)
        {
            var sintese = percentualFrequencia >= ObterFrequenciaMedia(disciplina) ? 
                        SinteseEnum.Frequente : SinteseEnum.NaoFrequente;

            return new SinteseDto()
            {
                SinteseId = sintese,
                SinteseNome = sintese.Name()
            };
        }

        private PeriodoEscolarDto BuscaPeriodo(int anoLetivo, Modalidade modalidadeCodigo, int bimestre, int semestre)
        {
            var tipoCalendario = consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio);
            var periodo = consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id).Periodos.FirstOrDefault(p => p.Bimestre == bimestre);

            // TODO alterar verificação para checagem de periodo de fechamento e reabertura do fechamento depois de implementado
            if (DateTime.Now < periodo.PeriodoInicio || DateTime.Now > periodo.PeriodoFim)
                throw new NegocioException($"Período do {bimestre}º Bimestre não está aberto");

            return periodo;
        }

        private Turma BuscaTurma(string turmaId)
        {
            var turma = repositorioTurma.ObterPorCodigo(turmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(AlunoPorTurmaResposta aluno, string disciplinaId, PeriodoEscolarDto bimestre, int percentualAlerta, int percentualCritico)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(aluno.CodigoAluno, disciplinaId, bimestre.PeriodoInicio, bimestre.PeriodoFim, TipoFrequenciaAluno.PorDisciplina);
            // Frequencia não calculada
            if (frequenciaAluno == null)
                return null;

            int percentualFrequencia = (int)Math.Round(frequenciaAluno.PercentualFrequencia, 0);
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

        public double ObterFrequenciaMedia(DisciplinaDto disciplina)
        {
            if (_mediaFrequencia == 0)
            {
                if (disciplina.Regencia || !disciplina.LancaNota)
                    _mediaFrequencia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse));
                else
                    _mediaFrequencia = double.Parse(repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.CompensacaoAusenciaPercentualFund2));
            }

            return _mediaFrequencia;
        }

        public async Task<double> ObterFrequenciaGeralAluno(string alunoCodigo)
        {
            var frequenciaAlunoPeriodos = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaGeralAluno(alunoCodigo);

            if (frequenciaAlunoPeriodos == null || !frequenciaAlunoPeriodos.Any())
                return 100;

            var frequenciaAluno = new FrequenciaAluno()
            {
                TotalAulas = frequenciaAlunoPeriodos.Sum(f => f.TotalAulas),
                TotalAusencias = frequenciaAlunoPeriodos.Sum(f => f.TotalAusencias),
                TotalCompensacoes = frequenciaAlunoPeriodos.Sum(f => f.TotalCompensacoes),
            };

            return frequenciaAluno.PercentualFrequencia;
        }
    }
}