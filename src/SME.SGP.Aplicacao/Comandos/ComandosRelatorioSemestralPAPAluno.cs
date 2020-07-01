using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosRelatorioSemestralPAPAluno : IComandosRelatorioSemestralPAPAluno
    {
        private readonly IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IComandosRelatorioSemestralTurmaPAP comandosRelatorioSemestral;
        private readonly IConsultasRelatorioSemestralTurmaPAP consultasRelatorioSemestral;
        private readonly IComandosRelatorioSemestralPAPAlunoSecao comandosRelatorioSemestralAlunoSecao;
        private readonly IConsultasTurma consultasTurma;
        private readonly IUnitOfWork unitOfWork;

        public ComandosRelatorioSemestralPAPAluno(IRepositorioRelatorioSemestralPAPAluno repositorioRelatorioSemestralAluno,
                                               IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                               IComandosRelatorioSemestralTurmaPAP comandosRelatorioSemestral,
                                               IConsultasRelatorioSemestralTurmaPAP consultasRelatorioSemestral,
                                               IComandosRelatorioSemestralPAPAlunoSecao comandosRelatorioSemestralAlunoSecao,
                                               IConsultasTurma consultasTurma,
                                               IUnitOfWork unitOfWork)
        {
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.comandosRelatorioSemestral = comandosRelatorioSemestral ?? throw new ArgumentNullException(nameof(comandosRelatorioSemestral));
            this.consultasRelatorioSemestral = consultasRelatorioSemestral ?? throw new ArgumentNullException(nameof(consultasRelatorioSemestral));
            this.comandosRelatorioSemestralAlunoSecao = comandosRelatorioSemestralAlunoSecao ?? throw new ArgumentNullException(nameof(comandosRelatorioSemestralAlunoSecao));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaRelatorioSemestralAlunoDto> Salvar(string alunoCodigo, string turmaCodigo, int semestre, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto)
        {
            var turma = await ObterTurma(turmaCodigo);
            await ValidarPersistenciaTurmaSemestre(turma, semestre);

            var relatorioSemestralAluno = relatorioSemestralAlunoDto.RelatorioSemestralAlunoId > 0 ?
                await repositorioRelatorioSemestralAluno.ObterCompletoPorIdAsync(relatorioSemestralAlunoDto.RelatorioSemestralAlunoId) :
                await NovoRelatorioSemestralAluno(relatorioSemestralAlunoDto.RelatorioSemestralId, alunoCodigo, turma, semestre, relatorioSemestralAlunoDto);

            using(var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    // Relatorio Semestral
                    if (relatorioSemestralAluno.RelatorioSemestralTurmaPAP != null)
                    {
                        await comandosRelatorioSemestral.SalvarAsync(relatorioSemestralAluno.RelatorioSemestralTurmaPAP);
                        relatorioSemestralAluno.RelatorioSemestralTurmaPAPId = relatorioSemestralAluno.RelatorioSemestralTurmaPAP.Id;
                    }

                    // Relatorio Semestral Aluno
                    await repositorioRelatorioSemestralAluno.SalvarAsync(relatorioSemestralAluno);

                    foreach(var secaoRelatorio in relatorioSemestralAlunoDto.Secoes)
                    {
                        var secaoRelatorioAluno = relatorioSemestralAluno.Secoes.FirstOrDefault(c => c.SecaoRelatorioSemestralPAPId == secaoRelatorio.Id);
                        if (secaoRelatorioAluno != null)
                        {
                            secaoRelatorioAluno.RelatorioSemestralPAPAlunoId = relatorioSemestralAluno.Id;
                            secaoRelatorioAluno.Valor = secaoRelatorio.Valor;

                            // Relatorio Semestral Aluno x Secao
                            await comandosRelatorioSemestralAlunoSecao.SalvarAsync(secaoRelatorioAluno);
                        }
                    }

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }            
            }

            return MapearParaAuditorio(relatorioSemestralAluno);
        }

        private async Task ValidarPersistenciaTurmaSemestre(Turma turma, int semestre)
        {
            var bimestre = await ObterBimestreAtual(turma);
            if ((semestre == 1 && bimestre != 2) || (semestre == 2 && bimestre != 4))
                throw new NegocioException("Não é possível salvar os dados pois o período não esta em aberto!");
        }

        private async Task<int> ObterBimestreAtual(Turma turma)
        {
            var bimestreAtual = await repositorioPeriodoEscolar.ObterBimestreAtualAsync(turma.CodigoTurma, turma.ModalidadeTipoCalendario, DateTime.Today);
            if (bimestreAtual == 0)
                throw new NegocioException("Não foi possível identificar o bimestre atual");

            return bimestreAtual;
        }

        private AuditoriaRelatorioSemestralAlunoDto MapearParaAuditorio(RelatorioSemestralPAPAluno relatorioSemestralAluno)
            => new AuditoriaRelatorioSemestralAlunoDto()
            {
                RelatorioSemestralId = relatorioSemestralAluno.RelatorioSemestralTurmaPAPId,
                RelatorioSemestralAlunoId = relatorioSemestralAluno.Id,
                Auditoria = (AuditoriaDto)relatorioSemestralAluno
            };

        private async Task<RelatorioSemestralPAPAluno> NovoRelatorioSemestralAluno(long relatorioSemestralId, string alunoCodigo, Turma turma, int semestre, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto)
        {
            var relatorioSemestral = relatorioSemestralId > 0 ?
                await consultasRelatorioSemestral.ObterPorIdAsync(relatorioSemestralId) :
                NovoRelatorioSemestral(turma, semestre);

            var novoRelatorioAluno = new RelatorioSemestralPAPAluno()
            {
                AlunoCodigo = alunoCodigo,
                RelatorioSemestralTurmaPAP = relatorioSemestral,
            };

            foreach (var secaoRelatorio in relatorioSemestralAlunoDto.Secoes)
                novoRelatorioAluno.Secoes.Add(new RelatorioSemestralPAPAlunoSecao()
                {
                    RelatorioSemestralPAPAluno = novoRelatorioAluno,
                    SecaoRelatorioSemestralPAPId = secaoRelatorio.Id,
                });

            return novoRelatorioAluno;
        }

        private RelatorioSemestralTurmaPAP NovoRelatorioSemestral(Turma turma, int semestre)
        {
            return new RelatorioSemestralTurmaPAP()
            {
                TurmaId = turma.Id,
                Semestre = semestre
            };
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await consultasTurma.ObterPorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }
    }
}
