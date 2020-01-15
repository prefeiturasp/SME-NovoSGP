using SME.Background.Core;
using SME.SGP.Aplicacao;
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
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ServicoCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                          IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
                                          IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequencia,
                                          IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                          IRepositorioTipoCalendario repositorioTipoCalendario,
                                          IServicoEOL servicoEOL,
                                          IRepositorioTurma repositorioTurma,
                                          IUnitOfWork unitOfWork)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Salvar(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            // Busca dados da turma
            var turma = BuscaTurma(compensacaoDto.TurmaId);

            // Valida mesma compensação no ano
            var compensacaoExistente = await repositorioCompensacaoAusencia.ObterPorAnoTurmaENome(turma.AnoLetivo, turma.Id, compensacaoDto.Atividade, id);
            if (compensacaoExistente != null)
                throw new NegocioException("Já existe essa compensação cadastrada para turma no ano letivo.");

            // Consiste periodo
            var periodo = BuscaPeriodo(turma.AnoLetivo, turma.ModalidadeCodigo, compensacaoDto.Bimestre, turma.Semestre);

            // Carrega dasdos da disciplina no EOL
            ConsisteDisciplina(long.Parse(compensacaoDto.DisciplinaId), compensacaoDto.DisciplinasRegenciaIds);

            // Persiste os dados
            var compensacao = MapearEntidade(id, compensacaoDto);
            compensacao.TurmaId = turma.Id;
            compensacao.AnoLetivo = turma.AnoLetivo;

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioCompensacaoAusencia.SalvarAsync(compensacao);
                await GravarCompensacaoAlunos(id > 0, compensacao.Id, compensacaoDto.TurmaId, compensacaoDto.DisciplinaId, compensacaoDto.Alunos, periodo);
                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private void ConsisteDisciplina(long disciplinaId, IEnumerable<string> disciplinasRegenciaIds)
        {
            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId });

            if (!disciplinasEOL.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");

            var disciplina = disciplinasEOL.FirstOrDefault();

            if (disciplina.Regencia && ((disciplinasRegenciaIds == null) || !disciplinasRegenciaIds.Any()))
                throw new NegocioException("Regência de classe deve informar a(s) disciplina(s) relacionadas a esta atividade.");

        }

        private PeriodoEscolarDto BuscaPeriodo(int anoLetivo, Modalidade modalidadeCodigo, int bimestre, int semestre)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio);
            var periodo = consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id).Periodos.FirstOrDefault(p => p.Bimestre == bimestre);
            // TODO alterar verificação para checagem de periodo de fechamento e reabertura do fechamento depois de implementado
            if (DateTime.Now < periodo.PeriodoInicio || DateTime.Now > periodo.PeriodoFim)
                throw new NegocioException($"Período do {bimestre}º Bimestre não esta aberto");

            return periodo;
        }

        private Turma BuscaTurma(string turmaId)
        {
            var turma = repositorioTurma.ObterPorId(turmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private async Task GravarCompensacaoAlunos(bool alteracao, long compensacaoId, string turmaId, string disciplinaId, IEnumerable<CompensacaoAusenciaAlunoDto> alunosDto, PeriodoEscolarDto periodo)
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
                var frequenciaAluno = repositorioFrequencia.ObterPorAlunoDisciplinaData(aluno.CodigoAluno, disciplinaId, periodo.PeriodoFim);
                if (frequenciaAluno == null)
                    throw new NegocioException($"Aluno [{aluno.CodigoAluno}] não possui ausência registrada. Não é possível incluí-lo na compensação.");

                var faltasNaoCompensadas = frequenciaAluno.NumeroFaltasNaoCompensadas - aluno.QuantidadeFaltasCompensadas;

                var alunoDto = alunosDto.FirstOrDefault(a => a.AlunoCodigo == aluno.CodigoAluno);
                if (alunoDto.QtdFaltasCompensadas > faltasNaoCompensadas)
                    throw new NegocioException($"O aluno(a) [{alunoDto.AlunoCodigo}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas.");

                aluno.QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas;
                listaPersistencia.Add(aluno);
            }

            // adiciona os alunos novos
            foreach(var alunoDto in alunosDto.Where(d => !alunos.Any(a => a.CodigoAluno == d.AlunoCodigo)))
            {
                var frequenciaAluno = repositorioFrequencia.ObterPorAlunoDisciplinaData(alunoDto.AlunoCodigo, disciplinaId, periodo.PeriodoFim);

                if (alunoDto.QtdFaltasCompensadas > frequenciaAluno.NumeroFaltasNaoCompensadas)
                    throw new NegocioException($"O aluno(a) [{alunoDto.AlunoCodigo}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas.");

                listaPersistencia.Add(MapearCompensacaoAlunoEntidade(compensacaoId, alunoDto));
            }

            listaPersistencia.ForEach(aluno => repositorioCompensacaoAusenciaAluno.Salvar(aluno));

            // Recalcula Frequencia dos alunos envolvidos na Persistencia
            var codigosAlunos = listaPersistencia.Select(a => a.CodigoAluno).ToList();
            Cliente.Executar<IServicoCalculoFrequencia>(c => c.CalcularFrequenciaPorTurma(codigosAlunos, periodo.PeriodoFim, turmaId, disciplinaId));
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

            return compensacao;
        }
    }
}
