using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasse : IServicoConselhoClasse
    {
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IUnitOfWork unitOfWork;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                     IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                     IConsultasConselhoClasse consultasConselhoClasse,
                                     IUnitOfWork unitOfWork)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> SalvarConselhoClasseAlunoNota(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId)
        {
            var conselhoClasseNota = new ConselhoClasseNota();
            if (fechamentoTurmaId == 0)
            {
                throw new NegocioException("Não existe fechamento de turma para o conselho de classe");
            }
            else
            {
                var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
                if (fechamentoTurma.PeriodoEscolarId.HasValue)
                {
                    // Fechamento Bimestral
                    if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                        throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
                }
                else
                {
                    // Fechamento Final
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2)
                        throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
            }

            unitOfWork.IniciarTransacao();
            try
            {
                if (conselhoClasseId == 0)
                {

                    var conselhoClasse = new ConselhoClasse();
                    conselhoClasse.FechamentoTurmaId = fechamentoTurmaId;
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                    long conselhoClasseAlunoId = await SalvarConselhoClasseAluno(conselhoClasse.Id, alunoCodigo);

                    conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                    unitOfWork.PersistirTransacao();
                }
                else
                {
                    var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAsync(conselhoClasseId, alunoCodigo);

                    var conselhoClasseAlunoId = conselhoClasseAluno != null ? conselhoClasseAluno.Id : await SalvarConselhoClasseAluno(conselhoClasseId, alunoCodigo);

                    conselhoClasseNota = await repositorioConselhoClasseNota.ObterPorConselhoClasseAlunoComponenteCurricular(conselhoClasseAlunoId, conselhoClasseNotaDto.ComponenteCurricularCodigo);

                    if (conselhoClasseNota == null)
                    {
                        conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);
                    }

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                }
            }

            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
            return (AuditoriaDto)conselhoClasseNota;
        }

        private async Task<long> SalvarConselhoClasseAluno(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAluno = new ConselhoClasseAluno()
            {
                AlunoCodigo = alunoCodigo,
                ConselhoClasseId = conselhoClasseId
            };

            return await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
        }

        private ConselhoClasseNota ObterConselhoClasseNota(ConselhoClasseNotaDto conselhoClasseNotaDto, long conselhoClasseAlunoId)
        {
            return new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                ComponenteCurricularCodigo = conselhoClasseNotaDto.ComponenteCurricularCodigo,
                Nota = conselhoClasseNotaDto.Nota.Value,
                ConceitoId = conselhoClasseNotaDto.Conceito,
                Justificativa = conselhoClasseNotaDto.Justificativa,
            };
        }

        Task<AuditoriaConselhoClasseAlunoDto> IServicoConselhoClasse.SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            throw new NotImplementedException();
        }
    }
}