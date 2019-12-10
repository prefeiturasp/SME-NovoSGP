using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Interfaces;
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
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoFrequencia servicoFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;


        public ConsultasFrequencia(IServicoFrequencia servicoFrequencia,
                                   IServicoEOL servicoEOL,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                   IRepositorioAula repositorioAula,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                   IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new ArgumentNullException(nameof(servicoFrequencia));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
                throw new NegocioException("Aula não encontrada.");

            var alunosDaTurma = await servicoEOL.ObterAlunosPorTurma(aula.TurmaId);
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

            foreach (var aluno in alunosDaTurma)
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
                registroFrequenciaAluno.Marcador = await ObterMarcadorAluno(aluno, bimestre);

                // Indicativo de frequencia do aluno
                registroFrequenciaAluno.IndicativoFrequencia = ObterIndicativoFrequencia(aluno, aula.DisciplinaId, bimestre, percentualAlerta, percentualCritico);

                if (aula.PermiteRegistroFrequencia(turma))
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

            var percentualFrequencia = (int)(frequenciaAluno.TotalAusencias / frequenciaAluno.TotalAulas * 100);

            // Critico
            if (percentualFrequencia <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequencia };

            // Alerta
            if (percentualFrequencia <= percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequencia };

            return null;
        }

        public async Task<MarcadorFrequenciaDto> ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolarDto bimestre)
        {
            MarcadorFrequenciaDto marcador = null;
                        
            string dataSituacao = $"{aluno.DataSituacao.Day}/{aluno.DataSituacao.Month}/{aluno.DataSituacao.Year}";
            switch (aluno.CodigoSituacaoMatricula)
            {
                case SituacaoMatriculaAluno.Ativo:
                    // Macador "Novo" durante 15 dias se iniciou depois do inicio do bimestre
                    if ((aluno.DataSituacao > bimestre.PeriodoInicio) && (aluno.DataSituacao.AddDays(15) <= DateTime.Now.Date))
                        marcador = new MarcadorFrequenciaDto()
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = $"Estudante Novo: Data da Matricula {dataSituacao}"
                        };
                    break;
                case SituacaoMatriculaAluno.Transferido:
                    var detalheEscola = aluno.Transferencia_Interna ? 
                                        $"para escola {aluno.EscolaTransferencia} e turma {aluno.TurmaTransferencia}" :
                                        "para outras redes";

                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Transferido,
                        Descricao = $"Estudante transferido: {detalheEscola} em {dataSituacao}"
                    };

                    break;
                case SituacaoMatriculaAluno.RemanejadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Remanejado,
                        Descricao = $"Estudante remanejado: turma {aluno.TurmaRemanejamento} em {dataSituacao}"
                    };

                    break;
                case SituacaoMatriculaAluno.Desistente:
                case SituacaoMatriculaAluno.VinculoIndevido:
                case SituacaoMatriculaAluno.Falecido:
                case SituacaoMatriculaAluno.NaoCompareceu:
                case SituacaoMatriculaAluno.Deslocamento:
                case SituacaoMatriculaAluno.Cessado:
                case SituacaoMatriculaAluno.ReclassificadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Inativo,
                        Descricao = $"Aluno inativo em {dataSituacao}"
                    };

                    break;
                default:
                    break;
            }

            return marcador;
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