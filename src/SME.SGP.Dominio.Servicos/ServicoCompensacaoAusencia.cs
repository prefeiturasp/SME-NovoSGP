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
    public class ServicoCompensacaoAusencia : IServicoCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                          IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno,
                                          IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia,
                                          IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequencia,
                                          IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                          IConsultasTurma consultasTurma,
                                          IRepositorioTipoCalendario repositorioTipoCalendario,
                                          IServicoEol servicoEOL,
                                          IServicoUsuario servicoUsuario,
                                          IRepositorioTurma repositorioTurma,
                                          IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia,
                                          IUnitOfWork unitOfWork)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new System.ArgumentNullException(nameof(consultasTurma));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioNotificacaoCompensacaoAusencia = repositorioNotificacaoCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioNotificacaoCompensacaoAusencia));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Salvar(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            // Busca dados da turma
            var turma = await BuscaTurma(compensacaoDto.TurmaId);

            // Consiste periodo
            var periodo = await BuscaPeriodo(turma, compensacaoDto.Bimestre);

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            await ValidaProfessorPodePersistirTurma(compensacaoDto.TurmaId, usuario.CodigoRf, periodo.PeriodoFim);

            // Valida mesma compensação no ano
            var compensacaoExistente = await repositorioCompensacaoAusencia.ObterPorAnoTurmaENome(turma.AnoLetivo, turma.Id, compensacaoDto.Atividade, id);
            if (compensacaoExistente != null)
            {
                throw new NegocioException($"Já existe essa compensação cadastrada para turma no ano letivo.");
            }

            CompensacaoAusencia compensacaoBanco = new CompensacaoAusencia();

            if (id > 0)
                compensacaoBanco = repositorioCompensacaoAusencia.ObterPorId(id);

            // Carrega dasdos da disciplina no EOL
            ConsisteDisciplina(long.Parse(compensacaoDto.DisciplinaId), compensacaoDto.DisciplinasRegenciaIds, compensacaoBanco.Migrado);

            // Persiste os dados
            var compensacao = MapearEntidade(compensacaoDto, compensacaoBanco);
            compensacao.TurmaId = turma.Id;
            compensacao.AnoLetivo = turma.AnoLetivo;

            List<string> codigosAlunosCompensacao = new List<string>();
            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioCompensacaoAusencia.SalvarAsync(compensacao);
                await GravarDisciplinasRegencia(id > 0, compensacao.Id, compensacaoDto.DisciplinasRegenciaIds);
                codigosAlunosCompensacao = await GravarCompensacaoAlunos(id > 0, compensacao.Id, compensacaoDto.TurmaId, compensacaoDto.DisciplinaId, compensacaoDto.Alunos, periodo);
                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }

            if (codigosAlunosCompensacao.Any())
                Cliente.Executar<IServicoCalculoFrequencia>(c => c.CalcularFrequenciaPorTurma(codigosAlunosCompensacao, periodo.PeriodoFim, compensacaoDto.TurmaId, compensacaoDto.DisciplinaId));

            Cliente.Executar<IServicoNotificacaoFrequencia>(c => c.NotificarCompensacaoAusencia(compensacao.Id));
        }

        private void ConsisteDisciplina(long disciplinaId, IEnumerable<string> disciplinasRegenciaIds, bool registroMigrado)
        {
            var disciplinasEOL = servicoEOL.ObterDisciplinasPorIds(new long[] { disciplinaId });

            if (!disciplinasEOL.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");

            var disciplina = disciplinasEOL.FirstOrDefault();

            if (!registroMigrado && disciplina.Regencia && ((disciplinasRegenciaIds == null) || !disciplinasRegenciaIds.Any()))
                throw new NegocioException("Regência de classe deve informar a(s) disciplina(s) relacionadas a esta atividade.");

        }

        private async Task<PeriodoEscolarDto> BuscaPeriodo(Turma turma, int bimestre)
        {
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio);

            PeriodoEscolarDto periodo = null;
            // Eja possui 2 calendarios por ano
            if (turma.ModalidadeCodigo == Modalidade.EJA)
            {
                if (turma.Semestre == 1)
                    periodo = (await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id)).Periodos
                        .FirstOrDefault(p => p.Bimestre == bimestre && p.PeriodoInicio < new DateTime(turma.AnoLetivo, 6, 1));
                else
                    periodo = (await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id)).Periodos
                        .FirstOrDefault(p => p.Bimestre == bimestre && p.PeriodoFim > new DateTime(turma.AnoLetivo, 6, 1));
            }
            else
                periodo = (await consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id)).Periodos
                    .FirstOrDefault(p => p.Bimestre == bimestre);

            if (!await consultasTurma.TurmaEmPeriodoAberto(turma, DateTime.Today, bimestre, tipoCalendario: tipoCalendario))
                throw new NegocioException($"Período do {bimestre}º Bimestre não está aberto.");

            return periodo;
        }

        private async Task<Turma> BuscaTurma(string turmaId)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaId);
            if (turma == null)
                throw new NegocioException("Turma não localizada!");

            return turma;
        }

        private async Task GravarDisciplinasRegencia(bool alteracao, long compensacaoId, IEnumerable<string> disciplinasRegenciaIds)
        {
            if (disciplinasRegenciaIds == null)
                return;

            var listaPersistencia = new List<CompensacaoAusenciaDisciplinaRegencia>();
            IEnumerable<CompensacaoAusenciaDisciplinaRegencia> disciplinas = new List<CompensacaoAusenciaDisciplinaRegencia>();
            if (alteracao)
                disciplinas = await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacaoId);

            // Remove as disciplinas não existentes mais
            foreach (var disciplinaExcluida in disciplinas.Where(x => !disciplinasRegenciaIds.Any(d => d == x.DisciplinaId)))
            {
                disciplinaExcluida.Excluir();
                listaPersistencia.Add(disciplinaExcluida);
            }

            // Inclui as disciplinas novas
            foreach (var disciplinaId in disciplinasRegenciaIds)
            {
                listaPersistencia.Add(new CompensacaoAusenciaDisciplinaRegencia()
                {
                    CompensacaoAusenciaId = compensacaoId,
                    DisciplinaId = disciplinaId,
                    Excluido = false
                });
            }

            listaPersistencia.ForEach(disciplina => repositorioCompensacaoAusenciaDisciplinaRegencia.Salvar(disciplina));
        }

        private async Task<List<string>> GravarCompensacaoAlunos(bool alteracao, long compensacaoId, string turmaId, string disciplinaId, IEnumerable<CompensacaoAusenciaAlunoDto> alunosDto, PeriodoEscolarDto periodo)
        {
            var mensagensExcessao = new StringBuilder();

            List<CompensacaoAusenciaAluno> listaPersistencia = new List<CompensacaoAusenciaAluno>();
            IEnumerable<CompensacaoAusenciaAluno> alunos = new List<CompensacaoAusenciaAluno>();

            if (alteracao)
                alunos = await repositorioCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoId);

            // excluir os removidos da lista
            foreach (var alunoRemovido in alunos.Where(a => !alunosDto.Any(d => d.Id == a.CodigoAluno)))
            {
                alunoRemovido.Excluir();
                listaPersistencia.Add(alunoRemovido);
            }

            // altera as faltas compensadas
            foreach (var aluno in alunos.Where(a => !a.Excluido))
            {
                var frequenciaAluno = repositorioFrequencia.ObterPorAlunoDisciplinaData(aluno.CodigoAluno, disciplinaId, periodo.PeriodoFim);
                if (frequenciaAluno == null)
                {
                    mensagensExcessao.Append($"O aluno(a) [{aluno.CodigoAluno}] não possui ausência para compensar. ");
                    continue;
                }

                var faltasNaoCompensadas = frequenciaAluno.NumeroFaltasNaoCompensadas + aluno.QuantidadeFaltasCompensadas;

                var alunoDto = alunosDto.FirstOrDefault(a => a.Id == aluno.CodigoAluno);
                if (alunoDto.QtdFaltasCompensadas > faltasNaoCompensadas)
                {
                    mensagensExcessao.Append($"O aluno(a) [{alunoDto.Id}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas. ");
                    continue;
                }

                aluno.QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas;
                listaPersistencia.Add(aluno);
            }

            // adiciona os alunos novos
            foreach (var alunoDto in alunosDto.Where(d => !alunos.Any(a => a.CodigoAluno == d.Id)))
            {
                var frequenciaAluno = repositorioFrequencia.ObterPorAlunoDisciplinaData(alunoDto.Id, disciplinaId, periodo.PeriodoFim);
                if (frequenciaAluno == null)
                {
                    mensagensExcessao.Append($"O aluno(a) [{alunoDto.Id}] não possui ausência para compensar. ");
                    continue;
                }

                if (alunoDto.QtdFaltasCompensadas > frequenciaAluno.NumeroFaltasNaoCompensadas)
                {
                    mensagensExcessao.Append($"O aluno(a) [{alunoDto.Id}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas. ");
                    continue;
                }

                listaPersistencia.Add(MapearCompensacaoAlunoEntidade(compensacaoId, alunoDto));
            }

            if (!string.IsNullOrEmpty(mensagensExcessao.ToString()))
                throw new NegocioException(mensagensExcessao.ToString());

            listaPersistencia.ForEach(aluno => repositorioCompensacaoAusenciaAluno.Salvar(aluno));

            // Recalcula Frequencia dos alunos envolvidos na Persistencia
            return listaPersistencia.Select(a => a.CodigoAluno).ToList();
        }

        private CompensacaoAusenciaAluno MapearCompensacaoAlunoEntidade(long compensacaoId, CompensacaoAusenciaAlunoDto alunoDto)
            => new CompensacaoAusenciaAluno()
            {
                CompensacaoAusenciaId = compensacaoId,
                CodigoAluno = alunoDto.Id,
                QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas,
                Notificado = false,
                Excluido = false
            };


        private CompensacaoAusencia MapearEntidade(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            CompensacaoAusencia compensacaoBanco = new CompensacaoAusencia();

            if (id > 0)
                compensacaoBanco = repositorioCompensacaoAusencia.ObterPorId(id);

            return MapearEntidade(compensacaoDto, compensacaoBanco);
        }

        private CompensacaoAusencia MapearEntidade(CompensacaoAusenciaDto compensacaoDto, CompensacaoAusencia compensacao)
        {
            compensacao.DisciplinaId = compensacaoDto.DisciplinaId;
            compensacao.Bimestre = compensacaoDto.Bimestre;
            compensacao.Nome = compensacaoDto.Atividade;
            compensacao.Descricao = compensacaoDto.Descricao;

            return compensacao;
        }

        public async Task Excluir(long[] compensacoesIds)
        {
            var compensacoesExcluir = new List<CompensacaoAusencia>();
            var compensacoesAlunosExcluir = new List<CompensacaoAusenciaAluno>();
            var compensacoesDisciplinasExcluir = new List<CompensacaoAusenciaDisciplinaRegencia>();

            List<long> idsComErroAoExcluir = new List<long>();

            // Carrega lista de objetos a excluir marcando-los para exclusão
            foreach (var compensacaoId in compensacoesIds)
            {
                var compensacao = repositorioCompensacaoAusencia.ObterPorId(compensacaoId);
                compensacao.Excluir();
                compensacoesExcluir.Add(compensacao);

                var compensacoesAlunos = await repositorioCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoId);
                foreach (var compensacaoAluno in compensacoesAlunos)
                {
                    compensacaoAluno.Excluir();
                    compensacoesAlunosExcluir.Add(compensacaoAluno);
                }

                var compensacoesDisciplinas = await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacaoId);
                foreach (var compensacaoDisciplina in compensacoesDisciplinas)
                {
                    compensacaoDisciplina.Excluir();
                    compensacoesDisciplinasExcluir.Add(compensacaoDisciplina);
                }
            }

            // Excluir lista carregada
            foreach (var compensacaoExcluir in compensacoesExcluir)
            {
                var turma = await repositorioTurma.ObterTurmaComUeEDrePorId(compensacaoExcluir.TurmaId);
                var periodo = await BuscaPeriodo(turma, compensacaoExcluir.Bimestre);

                unitOfWork.IniciarTransacao();
                try
                {
                    // Exclui dependencias
                    var alunosDaCompensacao = compensacoesAlunosExcluir.Where(c => c.CompensacaoAusenciaId == compensacaoExcluir.Id).ToList();
                    alunosDaCompensacao.ForEach(c => repositorioCompensacaoAusenciaAluno.Salvar(c));

                    compensacoesDisciplinasExcluir.Where(c => c.CompensacaoAusenciaId == compensacaoExcluir.Id).ToList()
                        .ForEach(c => repositorioCompensacaoAusenciaDisciplinaRegencia.Salvar(c));

                    // Exclui compensação
                    await repositorioCompensacaoAusencia.SalvarAsync(compensacaoExcluir);
                    // Excluir notificações
                    repositorioNotificacaoCompensacaoAusencia.Excluir(compensacaoExcluir.Id);

                    unitOfWork.PersistirTransacao();

                    if (alunosDaCompensacao.Any())
                        Cliente.Executar<IServicoCalculoFrequencia>(c => c.CalcularFrequenciaPorTurma(alunosDaCompensacao.Select(a => a.CodigoAluno), periodo.PeriodoFim, turma.CodigoTurma, compensacaoExcluir.DisciplinaId));
                }
                catch (Exception)
                {
                    idsComErroAoExcluir.Add(compensacaoExcluir.Id);
                    unitOfWork.Rollback();
                }
            }

            if (idsComErroAoExcluir.Any())
                throw new NegocioException($"Não foi possível excluir as compensações de ids {string.Join(",", idsComErroAoExcluir)}");
        }

        private async Task ValidaProfessorPodePersistirTurma(string turmaId, string codigoRf, DateTime dataAula)
        {
            if (!await servicoEOL.ProfessorPodePersistirTurma(codigoRf, turmaId, dataAula.Local()))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }

        public async Task<string> Copiar(CompensacaoAusenciaCopiaDto compensacaoCopia)
        {
            var compensacaoOrigem = repositorioCompensacaoAusencia.ObterPorId(compensacaoCopia.CompensacaoOrigemId);
            if (compensacaoOrigem == null)
                throw new NegocioException("Compensação de origem não localizada com o identificador informado.");

            var turmasCopiadas = new StringBuilder("");
            var turmasComErro = new StringBuilder("");
            foreach (var turmaId in compensacaoCopia.TurmasIds)
            {
                var turma = await repositorioTurma.ObterPorCodigo(turmaId);
                CompensacaoAusenciaDto compensacaoDto = new CompensacaoAusenciaDto()
                {
                    TurmaId = turmaId,
                    Bimestre = compensacaoCopia.Bimestre,
                    DisciplinaId = compensacaoOrigem.DisciplinaId,
                    Atividade = compensacaoOrigem.Nome,
                    Descricao = compensacaoOrigem.Descricao,
                    DisciplinasRegenciaIds = new List<string>(),
                    Alunos = new List<CompensacaoAusenciaAlunoDto>()
                };

                var disciplinasRegencia = await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacaoOrigem.Id);
                if (disciplinasRegencia != null && disciplinasRegencia.Any())
                    compensacaoDto.DisciplinasRegenciaIds = disciplinasRegencia.Select(s => s.DisciplinaId);

                try
                {
                    await Salvar(0, compensacaoDto);
                    turmasCopiadas.Append(turmasCopiadas.ToString().Length > 0 ? ", " + turma.Nome : turma.Nome);
                }
                catch (Exception e)
                {
                    turmasComErro.AppendLine($"A cópia para a turma {turma.Nome} não foi realizada: {e.Message}\n");
                }
            }
            var respTurmasCopiadas = turmasCopiadas.ToString();
            var textoTurmas = respTurmasCopiadas.Contains(",") ? "as turmas" : "a turma";
            var respostaSucesso = respTurmasCopiadas.Length > 0 ? $"A cópia para {textoTurmas} {respTurmasCopiadas} foi realizada com sucesso" : "";
            var respTurmasComErro = turmasComErro.ToString();
            if (respTurmasComErro.Length > 0)
            {
                throw new NegocioException($"{respTurmasComErro} {respostaSucesso}");
            }
            return respostaSucesso;
        }
    }
}
