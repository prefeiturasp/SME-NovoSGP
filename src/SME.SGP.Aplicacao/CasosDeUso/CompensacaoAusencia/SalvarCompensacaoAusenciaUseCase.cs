using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaUseCase : AbstractUseCase, ISalvarCompensacaoAusenciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public SalvarCompensacaoAusenciaUseCase(IMediator mediator, IUnitOfWork unitOfWork, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task Executar(long id, CompensacaoAusenciaDto compensacaoDto)
        {
            try
            {
                // Busca dados da turma
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(compensacaoDto.TurmaId));
                if (turma.EhNulo())
                    throw new NegocioException("Turma não localizada!");

                // Consiste periodo
                var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(turma.ModalidadeCodigo, turma.AnoLetivo, turma.Semestre));
                var periodo = periodos?.SingleOrDefault(p => p.Bimestre == compensacaoDto.Bimestre);

                var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PermiteCompensacaoForaPeriodo, turma.AnoLetivo));
                if (parametroSistema is not { Ativo: true })
                {
                    var turmaEmPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, periodo.Bimestre, false, periodo.TipoCalendarioId));

                    if (!turmaEmPeriodoAberto)
                        throw new NegocioException($"Período do {periodo.Bimestre}º Bimestre não está aberto.");
                }

                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

                if (!usuario.EhGestorEscolar())
                    await ValidaProfessorPodePersistirTurma(compensacaoDto.TurmaId, usuario, periodo.PeriodoFim);

                var codigosComponentesConsiderados = new List<string>() { compensacaoDto.DisciplinaId };

                // Valida mesma compensação no ano                
                var compensacaoExistente = await mediator.Send(new ObterCompensacaoAusenciaPorAnoTurmaENomeQuery(turma.AnoLetivo, turma.Id, compensacaoDto.Atividade, id, codigosComponentesConsiderados.ToArray()));
                if (compensacaoExistente.NaoEhNulo())
                    throw new NegocioException($"Já existe essa compensação cadastrada para turma no ano letivo.");

                var permiteRegistroFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(long.Parse(codigosComponentesConsiderados.OrderBy(c => c.Length).First())));
                if (!permiteRegistroFrequencia)
                    throw new NegocioException(MensagemNegocioCompensacaoAusencia.COMPONENTE_CURRICULAR_NAO_PERMITE_REGISTRAR_FREQUENCIA);

                var compensacaoBanco = new Dominio.CompensacaoAusencia();
                if (id > 0)
                    compensacaoBanco = await mediator.Send(new ObterCompensacaoAusenciaPorIdQuery(id));

                // Carrega dasdos da disciplina no EOL
                await ConsisteDisciplina(long.Parse(codigosComponentesConsiderados.OrderBy(c => c.Length).First()), compensacaoDto.DisciplinasRegenciaIds, compensacaoBanco.Migrado);

                var descricaoAtual = compensacaoBanco.Descricao;

                // Persiste os dados
                var compensacao = MapearEntidade(compensacaoDto, compensacaoBanco);
                compensacao.TurmaId = turma.Id;
                compensacao.AnoLetivo = turma.AnoLetivo;

                unitOfWork.IniciarTransacao();

                await mediator.Send(new SalvarCompensacaoAusenciaCommand(compensacao));
                await GravarDisciplinasRegencia(id > 0, compensacao.Id, compensacaoDto.DisciplinasRegenciaIds, usuario);

                IEnumerable<string> codigosAlunosCompensacao = new List<string>();
                var ehAlteracao = id > 0;

                IEnumerable<CompensacaoAusenciaAluno> compensacoesJaExistentes;
                if (ehAlteracao)
                    compensacoesJaExistentes = await mediator.Send(new ObterCompensacaoAusenciaAlunoPorCompensacaoQuery(id));
                else
                    compensacoesJaExistentes = Enumerable.Empty<CompensacaoAusenciaAluno>();

                if (compensacaoDto.Alunos.Any() || (ehAlteracao && compensacoesJaExistentes.Any()))
                {
                    var compensacaoAusenciaAlunos = await GravarCompensacaoAlunos(compensacao.Id, turma, codigosComponentesConsiderados.ToArray(), compensacaoDto.Alunos, periodo, compensacoesJaExistentes);
                    codigosAlunosCompensacao = await GravarCompensacaoAlunoAulas(ehAlteracao, compensacao, turma, compensacaoAusenciaAlunos, compensacaoDto.Alunos);
                }

                unitOfWork.PersistirTransacao();

                await MoverRemoverExcluidos(compensacaoDto.Descricao, descricaoAtual);

                if (codigosAlunosCompensacao.Any())
                {
                    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(codigosAlunosCompensacao, periodo.PeriodoFim, compensacaoDto.TurmaId, compensacaoDto.DisciplinaId, periodo.MesesDoPeriodo().ToArray()));
                }

                if (await mediator.Send(new VerificaSeExisteParametroSistemaPorTipoQuery(TipoParametroSistema.GerarNotificacaoCadastroDeCompensacaoDeAusencia)))
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificarCompensacaoAusencia, new FiltroNotificacaoCompensacaoAusenciaDto(compensacao.Id), Guid.NewGuid(), usuario));
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível salvar a compensação de ausência", LogNivel.Critico, LogContexto.Geral, ex.Message, rastreamento: ex.StackTrace, excecaoInterna: ex.InnerException?.ToString()));
                throw;
            }
        }

        private async Task MoverRemoverExcluidos(string novo, string atual)
        {
            if (!string.IsNullOrEmpty(novo))
                await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.CompensacaoAusencia, atual, novo));

            if (!string.IsNullOrEmpty(atual))
                await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.CompensacaoAusencia.Name()));
        }

        private async Task GravarDisciplinasRegencia(bool alteracao, long compensacaoId, IEnumerable<string> disciplinasRegenciaIds, Usuario usuarioLogado)
        {
            if (disciplinasRegenciaIds.EhNulo())
                return;

            var listaPersistencia = new List<CompensacaoAusenciaDisciplinaRegencia>();
            IEnumerable<CompensacaoAusenciaDisciplinaRegencia> disciplinas = new List<CompensacaoAusenciaDisciplinaRegencia>();
            if (alteracao)
                disciplinas = await mediator.Send(new ObterCompensacaoAusenciaDisciplinaRegenciaPorCompensacaoQuery(compensacaoId));

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

            await SalvarDisciplinasRegencia(alteracao, usuarioLogado, listaPersistencia);
        }

        private async Task SalvarDisciplinasRegencia(bool alteracao, Usuario usuarioLogado, IEnumerable<CompensacaoAusenciaDisciplinaRegencia> listaPersistencia)
        {
            //Inserir Lista de novos registros quando alteração é false
            if (!alteracao && listaPersistencia.Any())
                await mediator.Send(new InserirVariosCompensacaoAusenciaRegenciaCommand(listaPersistencia, usuarioLogado));
            //Atualizar individualmente quando alteração é true
            else if (alteracao && listaPersistencia.Any())
            {
                foreach (var disciplina in listaPersistencia)
                    await mediator.Send(new SalvarCompensacaoAusenciaDiciplinaRegenciaCommand(disciplina));
            }
        }

        private async Task<IEnumerable<CompensacaoAusenciaAluno>> GravarCompensacaoAlunos(long compensacaoId, Turma turma, string[] disciplinasId, IEnumerable<CompensacaoAusenciaAlunoDto> compensacaoAusenciaAlunoDtos, PeriodoEscolar periodo, IEnumerable<CompensacaoAusenciaAluno> compensacoesJaExistentes)
        {
            var mensagensExcessao = new StringBuilder();
            var listaPersistencia = new List<CompensacaoAusenciaAluno>();

            // excluir os removidos da lista
            foreach (var alunoRemovido in compensacoesJaExistentes.Where(a => !compensacaoAusenciaAlunoDtos.Any(d => d.Id == a.CodigoAluno)))
            {
                alunoRemovido.Excluir();
                listaPersistencia.Add(alunoRemovido);
            }

            if (compensacaoAusenciaAlunoDtos.Any())
            {
                var obterFrequenciaPorListaDeAlunosDisciplinaData = await mediator
                    .Send(new ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery(compensacaoAusenciaAlunoDtos?.Select(x => x.Id).ToArray(), disciplinasId, periodo.Id, turma.CodigoTurma));

                // altera as faltas compensadas
                var alunosAlterarFaltasCompensada = compensacoesJaExistentes.Where(a => !a.Excluido);

                if (alunosAlterarFaltasCompensada.Any())
                {
                    var alunosCodigos = alunosAlterarFaltasCompensada?.Select(x => x.CodigoAluno);

                    var consultaAlunosAlterarFaltasCompensada = obterFrequenciaPorListaDeAlunosDisciplinaData
                        .Where(o => alunosCodigos.Contains(o.CodigoAluno) && disciplinasId.Contains(o.DisciplinaId) && o.PeriodoFim == periodo.PeriodoFim && o.TurmaId == turma.CodigoTurma);

                    foreach (var aluno in alunosAlterarFaltasCompensada)
                    {
                        var frequenciaAluno = consultaAlunosAlterarFaltasCompensada
                            .FirstOrDefault(x => x.CodigoAluno == aluno.CodigoAluno && disciplinasId.Contains(x.DisciplinaId) && x.PeriodoFim == periodo.PeriodoFim && x.TurmaId == turma.CodigoTurma);

                        if (frequenciaAluno.EhNulo())
                        {
                            mensagensExcessao.Append($"O aluno(a) [{aluno.CodigoAluno}] não possui ausência para compensar. ");
                            continue;
                        }

                        var faltasNaoCompensadas = frequenciaAluno.NumeroFaltasNaoCompensadas > 0
                            ? frequenciaAluno.NumeroFaltasNaoCompensadas + aluno.QuantidadeFaltasCompensadas
                            : aluno.QuantidadeFaltasCompensadas;

                        var alunoDto = compensacaoAusenciaAlunoDtos.FirstOrDefault(a => a.Id == aluno.CodigoAluno);
                        if (alunoDto.QtdFaltasCompensadas > faltasNaoCompensadas)
                        {
                            mensagensExcessao.Append(
                                $"O aluno(a) [{alunoDto.Id}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas. ");
                            continue;
                        }

                        aluno.QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas;
                        listaPersistencia.Add(aluno);
                    }
                }

                // adiciona os alunos novos
                var listaAlunosDto = compensacaoAusenciaAlunoDtos?.Where(d => !compensacoesJaExistentes.Any(a => a.CodigoAluno == d.Id && !a.Excluido));
                if (listaAlunosDto.Any())
                {
                    var listaIdsAluno = listaAlunosDto.Select(x => x.Id);
                    var consultaAlunosFrequencia = obterFrequenciaPorListaDeAlunosDisciplinaData
                        .Where(c => listaIdsAluno.Contains(c.CodigoAluno) && disciplinasId.Contains(c.DisciplinaId) && c.PeriodoFim == periodo.PeriodoFim && c.TurmaId == turma.CodigoTurma);

                    foreach (var alunoDto in listaAlunosDto)
                    {
                        var frequenciaAluno = consultaAlunosFrequencia?
                            .FirstOrDefault(x => x.CodigoAluno == alunoDto.Id && disciplinasId.Contains(x.DisciplinaId) && x.TurmaId == turma.CodigoTurma);

                        if (frequenciaAluno.EhNulo())
                        {
                            mensagensExcessao.Append($"O aluno(a) [{alunoDto.Id}] não possui ausência para compensar. ");
                            continue;
                        }

                        if (alunoDto.QtdFaltasCompensadas > frequenciaAluno.NumeroFaltasNaoCompensadas && frequenciaAluno.NumeroFaltasNaoCompensadas > 0)
                        {
                            mensagensExcessao.Append(
                                $"O aluno(a) [{alunoDto.Id}] possui apenas {frequenciaAluno.NumeroFaltasNaoCompensadas} faltas não compensadas. ");

                            continue;
                        }

                        listaPersistencia.Add(MapearCompensacaoAlunoEntidade(compensacaoId, alunoDto));
                    }
                }

                if (!string.IsNullOrEmpty(mensagensExcessao.ToString()))
                    throw new NegocioException(mensagensExcessao.ToString());
            }

            await SalvarCompensacaoAlunos(listaPersistencia);

            // Recalcula Frequencia dos alunos envolvidos na Persistencia
            return listaPersistencia;
        }

        private async Task SalvarCompensacaoAlunos(IEnumerable<CompensacaoAusenciaAluno> compensacaoAusenciaAlunos)
        {
            foreach (var compensacaoAusenciaAluno in compensacaoAusenciaAlunos)
            {
                compensacaoAusenciaAluno.Id = await mediator.Send(new SalvarCompensacaoAusenciaAlunoCommand(compensacaoAusenciaAluno));
            }
        }

        private async Task<IEnumerable<string>> GravarCompensacaoAlunoAulas(bool alteracao, CompensacaoAusencia compensacao, Turma turma, IEnumerable<CompensacaoAusenciaAluno> compensacaoAusenciaAlunos, IEnumerable<CompensacaoAusenciaAlunoDto> compensacaoAusenciaAlunosDto)
        {
            var codigosAlunosQtdeCompensacao = compensacaoAusenciaAlunos
                .Where(t => t.QuantidadeFaltasCompensadas > 0)
                .Select(t => new AlunoQuantidadeCompensacaoDto(t.CodigoAluno, t.QuantidadeFaltasCompensadas))
                .Distinct();

            if (turma.AnoLetivo < 2023)
                return codigosAlunosQtdeCompensacao.Select(t => t.CodigoAluno);

            var faltasNaoCompensadas = await mediator.Send(new ObterAusenciaParaCompensacaoQuery(
                compensacao.Id,
                turma.CodigoTurma,
                compensacao.DisciplinaId,
                compensacao.Bimestre,
                codigosAlunosQtdeCompensacao));

            IEnumerable<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas = new List<CompensacaoAusenciaAlunoAula>();
            if (alteracao)
                compensacaoAusenciaAlunoAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery(compensacao.Id));

            var listaPersistencia = new List<CompensacaoAusenciaAlunoAula>();
            foreach (var compensacaoAusenciaAluno in compensacaoAusenciaAlunos)
            {
                var compensacaoAusenciaAlunoDto = compensacaoAusenciaAlunosDto.FirstOrDefault(t => t.Id == compensacaoAusenciaAluno.CodigoAluno);
                if (compensacaoAusenciaAlunoDto.EhNulo())
                    continue;

                var faltasNaoCompensadasAluno = faltasNaoCompensadas.Where(t => t.CodigoAluno == compensacaoAusenciaAluno.CodigoAluno);
                if (compensacaoAusenciaAlunoDto.QtdFaltasCompensadas > faltasNaoCompensadasAluno.Count())
                    throw new NegocioException($"O aluno {compensacaoAusenciaAluno.CodigoAluno} possui {faltasNaoCompensadasAluno.Count()} faltas registradas");

                if (compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.EhNulo())
                    compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>();

                if (compensacaoAusenciaAluno.QuantidadeFaltasCompensadas > 0 && !compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Any())
                {
                    //-> caso o usuário não informe as aulas, é selecionado automaticamente conforme a sugestão.
                    foreach (var falta in faltasNaoCompensadasAluno.Where(t => t.Sugestao))
                    {
                        AdicionarCompensacaoAlunoAula(compensacaoAusenciaAlunoAulas, listaPersistencia, compensacaoAusenciaAluno, falta);
                    }
                }
                else if (compensacaoAusenciaAlunoDto.QtdFaltasCompensadas == compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Count())
                {
                    //-> caso o usuário selecionou as aulas.
                    foreach (var aulaDto in compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula)
                    {
                        var falta = faltasNaoCompensadas.FirstOrDefault(t => t.CodigoAluno == compensacaoAusenciaAluno.CodigoAluno && t.RegistroFrequenciaAlunoId == aulaDto.RegistroFrequenciaAlunoId);

                        AdicionarCompensacaoAlunoAula(compensacaoAusenciaAlunoAulas, listaPersistencia, compensacaoAusenciaAluno, falta);
                    }
                }
                else if (compensacaoAusenciaAlunoDto.QtdFaltasCompensadas > compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Count())
                {
                    //-> caso a quantidade de compensadas for maior que as aulas selecionadas
                    foreach (var aulaDto in compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula)
                    {
                        var falta = faltasNaoCompensadas.FirstOrDefault(t => t.CodigoAluno == compensacaoAusenciaAluno.CodigoAluno && t.RegistroFrequenciaAlunoId == aulaDto.RegistroFrequenciaAlunoId);

                        AdicionarCompensacaoAlunoAula(compensacaoAusenciaAlunoAulas, listaPersistencia, compensacaoAusenciaAluno, falta);
                    }

                    var diferenca = compensacaoAusenciaAlunoDto.QtdFaltasCompensadas - compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Count();

                    // -> adiciona a quantidade faltante pegando da mais antiga para a mais nova.
                    foreach (var falta in faltasNaoCompensadasAluno
                        .Where(t => !compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Any(x => x.RegistroFrequenciaAlunoId == t.RegistroFrequenciaAlunoId))
                        .OrderBy(t => t.DataAula)
                        .ThenBy(t => t.NumeroAula)
                        .Take(diferenca))
                    {
                        AdicionarCompensacaoAlunoAula(compensacaoAusenciaAlunoAulas, listaPersistencia, compensacaoAusenciaAluno, falta);
                    }
                }
                else if (compensacaoAusenciaAlunoDto.QtdFaltasCompensadas < compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Count())
                {
                    //-> adiciona a diferença pegando da mais antiga para a mais nova.
                    foreach (var falta in faltasNaoCompensadasAluno
                        .Where(t => compensacaoAusenciaAlunoDto.CompensacaoAusenciaAlunoAula.Any(x => x.RegistroFrequenciaAlunoId == t.RegistroFrequenciaAlunoId))
                        .OrderBy(t => t.DataAula)
                        .ThenBy(t => t.NumeroAula)
                        .Take(compensacaoAusenciaAlunoDto.QtdFaltasCompensadas))
                    {
                        AdicionarCompensacaoAlunoAula(compensacaoAusenciaAlunoAulas, listaPersistencia, compensacaoAusenciaAluno, falta);
                    }
                }
            }

            // Remove as aulas não existentes
            foreach (var compensacaoAusenciaAlunoAulaExcluir in compensacaoAusenciaAlunoAulas.Where(t => !listaPersistencia.Any(x => x.CompensacaoAusenciaAlunoId == t.CompensacaoAusenciaAlunoId && x.RegistroFrequenciaAlunoId == t.RegistroFrequenciaAlunoId)))
            {
                compensacaoAusenciaAlunoAulaExcluir.Excluir();
                listaPersistencia.Add(compensacaoAusenciaAlunoAulaExcluir);
            }

            if (listaPersistencia.Any())
                await SalvarCompensacaoAlunoAulas(listaPersistencia);

            return codigosAlunosQtdeCompensacao.Select(t => t.CodigoAluno);
        }

        private static void AdicionarCompensacaoAlunoAula(IEnumerable<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas, List<CompensacaoAusenciaAlunoAula> listaPersistencia, CompensacaoAusenciaAluno compensacaoAusenciaAluno, RegistroFaltasNaoCompensadaDto falta)
        {
            var compensacaoAusenciaAlunoAula = compensacaoAusenciaAlunoAulas
                                        .FirstOrDefault(t => t.CompensacaoAusenciaAlunoId == compensacaoAusenciaAluno.Id && t.RegistroFrequenciaAlunoId == falta.RegistroFrequenciaAlunoId);

            if (compensacaoAusenciaAlunoAula.EhNulo())
                compensacaoAusenciaAlunoAula = new CompensacaoAusenciaAlunoAula()
                {
                    CompensacaoAusenciaAlunoId = compensacaoAusenciaAluno.Id,
                    RegistroFrequenciaAlunoId = falta.RegistroFrequenciaAlunoId,
                    NumeroAula = falta.NumeroAula,
                    DataAula = falta.DataAula
                };

            compensacaoAusenciaAlunoAula.Restaurar();

            listaPersistencia.Add(compensacaoAusenciaAlunoAula);
        }

        private async Task SalvarCompensacaoAlunoAulas(IEnumerable<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas)
        {
            foreach (var compensacaoAusenciaAlunoAula in compensacaoAusenciaAlunoAulas)
            {
                await mediator.Send(new SalvarCompensacaoAusenciaAlunoAulaCommand(compensacaoAusenciaAlunoAula));
            }
        }

        private CompensacaoAusenciaAluno MapearCompensacaoAlunoEntidade(long compensacaoId, CompensacaoAusenciaAlunoDto alunoDto) => new CompensacaoAusenciaAluno()
        {
            CompensacaoAusenciaId = compensacaoId,
            CodigoAluno = alunoDto.Id,
            QuantidadeFaltasCompensadas = alunoDto.QtdFaltasCompensadas,
            Notificado = false,
            Excluido = false
        };

        private CompensacaoAusencia MapearEntidade(CompensacaoAusenciaDto compensacaoDto, CompensacaoAusencia compensacao)
        {
            compensacao.DisciplinaId = compensacaoDto.DisciplinaId;
            compensacao.Bimestre = compensacaoDto.Bimestre;
            compensacao.Nome = compensacaoDto.Atividade;
            compensacao.Descricao = compensacaoDto.Descricao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            return compensacao;
        }

        private async Task ConsisteDisciplina(long disciplinaId, IEnumerable<string> disciplinasRegenciaIds, bool registroMigrado)
        {
            var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { disciplinaId }));

            if (!disciplinasEOL.Any())
                throw new NegocioException("Componente curricular não encontrado no EOL.");

            var disciplina = disciplinasEOL.FirstOrDefault();

            if (!registroMigrado && disciplina.Regencia && ((disciplinasRegenciaIds.EhNulo()) || !disciplinasRegenciaIds.Any()))
                throw new NegocioException("Regência de classe deve informar o(s) componente(s) curricular(es) relacionados a esta atividade.");
        }

        private async Task ValidaProfessorPodePersistirTurma(string turmaId, Usuario usuario, DateTime dataAula)
        {
            if (!await PossuiPermissaoTurma(turmaId, usuario, dataAula))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma e data.");
        }

        private async Task<bool> PossuiPermissaoTurma(string turmaId, Usuario usuario, DateTime dataAula)
        {
            if (usuario.EhProfessorCj())
                return await PossuiAtribuicaoCJ(turmaId, usuario.CodigoRf);

            return await mediator.Send(
                new ProfessorPodePersistirTurmaQuery(usuario.CodigoRf, turmaId, dataAula.Local()));
        }

        private async Task<bool> PossuiAtribuicaoCJ(string turmaId, string codigoRf)
        {
            var componentes = await mediator.Send(new ObterDisciplinasPerfilCJQuery(turmaId, codigoRf));
            return componentes.NaoEhNulo() && componentes.Any();
        }

    }
}
