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
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioConselhoClasseNota repositorioConselhoClasseNota;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;
        private readonly IUnitOfWork unitOfWork;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IRepositorioConselhoClasseNota repositorioConselhoClasseNota,
                                     IConsultasConselhoClasse consultasConselhoClasse,
                                     IUnitOfWork unitOfWork)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId)
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

            try
            {
                if (conselhoClasseId == 0)
                {

                    var conselhoClasse = new ConselhoClasse();
                    conselhoClasse.FechamentoTurmaId = fechamentoTurmaId;

                    unitOfWork.IniciarTransacao();
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                    long conselhoClasseAlunoId = await SalvarConselhoClasseAlunoResumido(conselhoClasse.Id, alunoCodigo);

                    conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);
                    unitOfWork.PersistirTransacao();
                }
                else
                {
                    var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAsync(conselhoClasseId, alunoCodigo);
                    unitOfWork.IniciarTransacao();

                    var conselhoClasseAlunoId = conselhoClasseAluno != null ? conselhoClasseAluno.Id : await SalvarConselhoClasseAlunoResumido(conselhoClasseId, alunoCodigo);

                    conselhoClasseNota = await repositorioConselhoClasseNota.ObterPorConselhoClasseAlunoComponenteCurricularAsync(conselhoClasseAlunoId, conselhoClasseNotaDto.CodigoComponenteCurricular);

                    if (conselhoClasseNota == null)
                    {
                        conselhoClasseNota = ObterConselhoClasseNota(conselhoClasseNotaDto, conselhoClasseAlunoId);
                    }
                    else
                    {
                        conselhoClasseNota.Justificativa = conselhoClasseNotaDto.Justificativa;
                        conselhoClasseNota.Nota = conselhoClasseNotaDto.Nota.Value;
                        conselhoClasseNota.ConceitoId = conselhoClasseNotaDto.Conceito;
                    }

                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);

                    unitOfWork.PersistirTransacao();
                }
            }

            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
            return (AuditoriaDto)conselhoClasseNota;
        }

        private async Task<long> SalvarConselhoClasseAlunoResumido(long conselhoClasseId, string alunoCodigo)
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
                ComponenteCurricularCodigo = conselhoClasseNotaDto.CodigoComponenteCurricular,
                Nota = conselhoClasseNotaDto.Nota.Value,
                ConceitoId = conselhoClasseNotaDto.Conceito,
                Justificativa = conselhoClasseNotaDto.Justificativa,
            };
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(conselhoClasse.FechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Não foi possível localizar o fechamento da turma informado!");

            var conselhoClasseExistente = await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId);
            if (conselhoClasseExistente != null)
                throw new NegocioException($"Já existe um conselho de classe gerado para a turma {fechamentoTurma.Turma.Nome}!");

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

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
        }

        public async Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            // Se não existir conselho de classe para o fechamento gera
            if (conselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(conselhoClasseAluno.ConselhoClasse);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(conselhoClasseAluno.ConselhoClasse);

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return (AuditoriaConselhoClasseAlunoDto)conselhoClasseAluno;
        }
    }
}