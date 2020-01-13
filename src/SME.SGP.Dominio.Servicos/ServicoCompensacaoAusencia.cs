using SME.Background.Core;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCompensacaoAusencia: IServicoCompensacaoAusencia
    {

        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequencia;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ServicoCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                          IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
                                          IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequencia,
                                          IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                          IServicoEOL servicoEOL,
                                          IRepositorioTurma repositorioTurma,
                                          IUnitOfWork unitOfWork)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Salvar(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { long.Parse(compensacaoDto.DisciplinaId) });

            if (!disciplinasEOL.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");

            var disciplina = disciplinasEOL.FirstOrDefault();

            if (disciplina.Regencia && ((compensacaoDto.DisciplinasRegenciaIds == null) || !compensacaoDto.DisciplinasRegenciaIds.Any()))
                throw new NegocioException("Regência de classe deve informar a(s) disciplina(s) relacionadas a esta atividade.");

            var compensacao = MapearEntidade(id, compensacaoDto);
            await repositorioCompensacaoAusencia.SalvarAsync(compensacao);
            await GravarCompensacaoAlunos(id > 0, compensacao.Id, compensacaoDto.TurmaId, compensacaoDto.DisciplinaId, compensacaoDto.Alunos);
        }

        private async Task GravarCompensacaoAlunos(bool alteracao, long compensacaoId, string turmaId, string disciplinaId, IEnumerable<CompensacaoAusenciaAlunoDto> alunosDto)
        {
            List<CompensacaoAusenciaAluno> listaPersistencia = new List<CompensacaoAusenciaAluno>();
            IEnumerable<CompensacaoAusenciaAluno> alunos = new List<CompensacaoAusenciaAluno>();
            if (alteracao)
                alunos = await repositorioCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoId);

            // excluir os removidos da lista
            foreach(var alunoRemovido in alunos.Where(a => !alunosDto.Any(d => d.AlunoCodigo == a.CodigoAluno)))
            {
                alunoRemovido.Excluir();
                listaPersistencia.Add(alunoRemovido);
            }

            // altera as faltas compensadas
            foreach (var aluno in alunos)
            {
                aluno.QuantidadeFaltasCompensadas = alunosDto.FirstOrDefault(a => a.AlunoCodigo == aluno.CodigoAluno).QtdFaltasCompensadas;
                listaPersistencia.Add(aluno);
            }

            // adiciona os alunos novos
            foreach(var alunoDto in alunosDto.Where(d => !alunos.Any(a => a.CodigoAluno == d.AlunoCodigo)))
            {
                listaPersistencia.Add(MapearCompensacaoAlunoEntidade(compensacaoId, alunoDto));
            }

            unitOfWork.IniciarTransacao();
            listaPersistencia.ForEach(aluno => repositorioCompensacaoAusenciaAluno.Salvar(aluno));
            unitOfWork.PersistirTransacao();

            // Recalcula Frequencia dos alunos envolvidos na Persistencia
            var codigosAlunos = listaPersistencia.Select(a => a.CodigoAluno).ToList();
            Cliente.Executar<IServicoCalculoFrequencia>(c => c.CalcularFrequenciaPorTurma(codigosAlunos, DateTime.Now, turmaId, disciplinaId));
        }

        private CompensacaoAusenciaAluno MapearCompensacaoAlunoEntidade(long compensacaoId, CompensacaoAusenciaAlunoDto alunoDto)
            => new CompensacaoAusenciaAluno()
            { 
                CompensacaoAusenciaId = compensacaoId,
                CodigoAluno = alunoDto.AlunoCodigo,
                QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas,
                Notificado = false,
                Excluido = false
            };


        private CompensacaoAusencia MapearEntidade(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            CompensacaoAusencia compensacao = new CompensacaoAusencia();
            if (id > 0)
                compensacao = repositorioCompensacaoAusencia.ObterPorId(id);

            compensacao.DisciplinaId = compensacaoDto.DisciplinaId;
            compensacao.Bimestre = compensacaoDto.Bimestre;
            compensacao.Nome = compensacaoDto.Atividade;
            compensacao.Descricao = compensacaoDto.Descricao;

            var turma = repositorioTurma.ObterPorId(compensacaoDto.TurmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            compensacao.TurmaId = turma.Id;

            return compensacao;
        }
    }
}
