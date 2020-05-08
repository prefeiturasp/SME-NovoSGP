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
    public class ComandosRelatorioSemestralAluno : IComandosRelatorioSemestralAluno
    {
        private readonly IRepositorioRelatorioSemestralAluno repositorioRelatorioSemestralAluno;
        private readonly IComandosRelatorioSemestral comandosRelatorioSemestral;
        private readonly IConsultasRelatorioSemestral consultasRelatorioSemestral;
        private readonly IComandosRelatorioSemestralAlunoSecao comandosRelatorioSemestralAlunoSecao;
        private readonly IConsultasTurma consultasTurma;
        private readonly IUnitOfWork unitOfWork;

        public ComandosRelatorioSemestralAluno(IRepositorioRelatorioSemestralAluno repositorioRelatorioSemestralAluno,
                                               IComandosRelatorioSemestral comandosRelatorioSemestral,
                                               IConsultasRelatorioSemestral consultasRelatorioSemestral,
                                               IComandosRelatorioSemestralAlunoSecao comandosRelatorioSemestralAlunoSecao,
                                               IConsultasTurma consultasTurma,
                                               IUnitOfWork unitOfWork)
        {
            this.repositorioRelatorioSemestralAluno = repositorioRelatorioSemestralAluno ?? throw new ArgumentNullException(nameof(repositorioRelatorioSemestralAluno));
            this.comandosRelatorioSemestral = comandosRelatorioSemestral ?? throw new ArgumentNullException(nameof(comandosRelatorioSemestral));
            this.consultasRelatorioSemestral = consultasRelatorioSemestral ?? throw new ArgumentNullException(nameof(consultasRelatorioSemestral));
            this.comandosRelatorioSemestralAlunoSecao = comandosRelatorioSemestralAlunoSecao ?? throw new ArgumentNullException(nameof(comandosRelatorioSemestralAlunoSecao));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaRelatorioSemestralAlunoDto> Salvar(string alunoCodigo, string turmaCodigo, int semestre, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto)
        {
            var relatorioSemestralAluno = relatorioSemestralAlunoDto.RelatorioSemestralAlunoId > 0 ?
                await repositorioRelatorioSemestralAluno.ObterCompletoPorIdAsync(relatorioSemestralAlunoDto.RelatorioSemestralAlunoId) :
                await NovoRelatorioSemestralAluno(relatorioSemestralAlunoDto.RelatorioSemestralId, alunoCodigo, turmaCodigo, semestre, relatorioSemestralAlunoDto);

            using(var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    // Relatorio Semestral
                    if (relatorioSemestralAluno.RelatorioSemestral != null)
                    {
                        await comandosRelatorioSemestral.SalvarAsync(relatorioSemestralAluno.RelatorioSemestral);
                        relatorioSemestralAluno.RelatorioSemestralId = relatorioSemestralAluno.RelatorioSemestral.Id;
                    }

                    // Relatorio Semestral Aluno
                    await repositorioRelatorioSemestralAluno.SalvarAsync(relatorioSemestralAluno);

                    foreach(var secaoRelatorio in relatorioSemestralAlunoDto.Secoes)
                    {
                        var secaoRelatorioAluno = relatorioSemestralAluno.Secoes.FirstOrDefault(c => c.SecaoRelatorioSemestralId == secaoRelatorio.Id);
                        if (secaoRelatorioAluno != null)
                        {
                            secaoRelatorioAluno.RelatorioSemestralAlunoId = relatorioSemestralAluno.Id;
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

        private AuditoriaRelatorioSemestralAlunoDto MapearParaAuditorio(RelatorioSemestralAluno relatorioSemestralAluno)
            => new AuditoriaRelatorioSemestralAlunoDto()
            {
                RelatorioSemestralId = relatorioSemestralAluno.RelatorioSemestralId,
                RelatorioSemestralAlunoId = relatorioSemestralAluno.Id,
                Auditoria = (AuditoriaDto)relatorioSemestralAluno
            };

        private async Task<RelatorioSemestralAluno> NovoRelatorioSemestralAluno(long relatorioSemestralId, string alunoCodigo, string turmaCodigo, int semestre, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto)
        {
            var relatorioSemestral = relatorioSemestralId > 0 ?
                await consultasRelatorioSemestral.ObterPorIdAsync(relatorioSemestralId) :
                await NovoRelatorioSemestral(turmaCodigo, semestre);

            var novoRelatorioAluno = new RelatorioSemestralAluno()
            {
                AlunoCodigo = alunoCodigo,
                RelatorioSemestral = relatorioSemestral,
            };

            foreach (var secaoRelatorio in relatorioSemestralAlunoDto.Secoes)
                novoRelatorioAluno.Secoes.Add(new RelatorioSemestralAlunoSecao()
                {
                    RelatorioSemestralAluno = novoRelatorioAluno,
                    SecaoRelatorioSemestralId = secaoRelatorio.Id,
                });

            return novoRelatorioAluno;
        }

        private async Task<RelatorioSemestral> NovoRelatorioSemestral(string turmaCodigo, int semestre)
        {
            var turma = await ObterTurma(turmaCodigo);

            return new RelatorioSemestral()
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
