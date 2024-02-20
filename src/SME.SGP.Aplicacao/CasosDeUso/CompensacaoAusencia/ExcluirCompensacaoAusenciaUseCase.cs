using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaUseCase : AbstractUseCase, IExcluirCompensacaoAusenciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioCache repositorioCache;

        public ExcluirCompensacaoAusenciaUseCase(IMediator mediator, IUnitOfWork ofWork, IRepositorioCache repositorioCache) : base(mediator)
        {
            unitOfWork = ofWork ?? throw new ArgumentNullException(nameof(ofWork));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(long[] compensacoesIds)
        {
            var compensacoesExcluir = new List<CompensacaoAusencia>();
            var compensacoesAlunosExcluir = new List<CompensacaoAusenciaAluno>();
            var compensacoesDisciplinasExcluir = new List<CompensacaoAusenciaDisciplinaRegencia>();
            var compensacoesAlunoAulasExcluir = new List<CompensacaoAusenciaAlunoAula>();
            var listaCompensacaoDescricao = new List<string>();

            foreach (var compensacaoId in compensacoesIds)
            {
                var compensacao = await mediator.Send(new ObterCompensacaoAusenciaPorIdQuery(compensacaoId));
                listaCompensacaoDescricao.Add(compensacao.Descricao);
                compensacao.Excluir();
                compensacoesExcluir.Add(compensacao);

                compensacoesAlunosExcluir.AddRange(
                                            ObterCompensacoesAusenciaAlunoExclusao(
                                                  await ObterCompensacoesAusenciaAluno(compensacaoId)));

                compensacoesDisciplinasExcluir.AddRange(
                                                    ObterCompensacoesAusenciaDisciplinaRegenciaExclusao(
                                                        await ObterCompensacoesAusenciaDisciplinaRegencia(compensacaoId)));

                compensacoesAlunoAulasExcluir.AddRange(
                                                    ObterCompensacoesAusenciaAlunoAulaExclusao(
                                                        await ObterCompensacoesAusenciaAlunoAula(compensacaoId)));
            }

            var idsComErroAoExcluir = await ExcluirCompensacoes(compensacoesExcluir, 
                                                                compensacoesAlunosExcluir, 
                                                                compensacoesAlunoAulasExcluir, 
                                                                compensacoesDisciplinasExcluir);
            
            if (listaCompensacaoDescricao.NaoEhNulo() && listaCompensacaoDescricao.Any())
            {
                foreach (var item in listaCompensacaoDescricao)
                {
                    await mediator.Send(
                        new DeletarArquivoDeRegistroExcluidoCommand(item, TipoArquivo.CompensacaoAusencia.Name()));
                }
            }

            if (idsComErroAoExcluir.Any())
                throw new NegocioException($"Não foi possível excluir as compensações de ids {string.Join(",", idsComErroAoExcluir)}");

            return true;
        }

        private Task<IEnumerable<CompensacaoAusenciaAlunoAula>> ObterCompensacoesAusenciaAlunoAula(long compensacaoId)
        {
            return mediator.Send(new ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery(compensacaoId));
        }

        private Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> ObterCompensacoesAusenciaDisciplinaRegencia(long compensacaoId)
        {
            return mediator.Send(new ObterCompensacaoAusenciaDisciplinaRegenciaPorIdQuery(compensacaoId));
        }

        private Task<IEnumerable<CompensacaoAusenciaAluno>> ObterCompensacoesAusenciaAluno(long compensacaoId)
        {
            return mediator.Send(new ObterCompensacaoAusenciaAlunoPorCompensacaoQuery(compensacaoId));
        }

        private IEnumerable<CompensacaoAusenciaAlunoAula> ObterCompensacoesAusenciaAlunoAulaExclusao(IEnumerable<CompensacaoAusenciaAlunoAula> compensacoes)
        {
            foreach (var compensacao in compensacoes)
            {
                compensacao.Excluir();
                yield return compensacao;
            }
        }

        private IEnumerable<CompensacaoAusenciaDisciplinaRegencia> ObterCompensacoesAusenciaDisciplinaRegenciaExclusao(IEnumerable<CompensacaoAusenciaDisciplinaRegencia> compensacoes)
        {
            foreach (var compensacao in compensacoes)
            {
                compensacao.Excluir();
                yield return compensacao;
            }
        }

        private IEnumerable<CompensacaoAusenciaAluno> ObterCompensacoesAusenciaAlunoExclusao(IEnumerable<CompensacaoAusenciaAluno> compensacoes)
        {
            foreach (var compensacao in compensacoes)
            {
                compensacao.Excluir();
                yield return compensacao;
            }
        }

        private async Task<List<long>> ExcluirCompensacoes(List<CompensacaoAusencia> compensacoes, 
                                               List<CompensacaoAusenciaAluno> compensacoesAlunos,
                                               List<CompensacaoAusenciaAlunoAula> compensacoesAlunoAulas,
                                                List<CompensacaoAusenciaDisciplinaRegencia> compensacoesDisciplinas)
        {
            var idsComErroAoExcluir = new List<long>();
            foreach (var compensacaoExcluir in compensacoes)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(compensacaoExcluir.TurmaId));
                var periodo = await BuscaPeriodo(turma, compensacaoExcluir.Bimestre);

                unitOfWork.IniciarTransacao();
                try
                {
                    var alunosDaCompensacao = compensacoesAlunos.Where(c => c.CompensacaoAusenciaId == compensacaoExcluir.Id);
                    foreach (var compensacaoAusenciaAluno in alunosDaCompensacao)
                    {
                        foreach (var compensacaoAlunoAulaExcluir in compensacoesAlunoAulas.Where(c => c.CompensacaoAusenciaAlunoId == compensacaoAusenciaAluno.Id))
                        {
                            await mediator.Send(new SalvarCompensacaoAusenciaAlunoAulaCommand(compensacaoAlunoAulaExcluir));
                        }

                        await mediator.Send(new SalvarCompensacaoAusenciaAlunoCommand(compensacaoAusenciaAluno));
                    }

                    foreach (var compensacaoDisciplinaRegenciaExcluir in compensacoesDisciplinas.Where(c => c.CompensacaoAusenciaId == compensacaoExcluir.Id))
                    {
                        await mediator.Send(new SalvarCompensacaoAusenciaDiciplinaRegenciaCommand(compensacaoDisciplinaRegenciaExcluir));
                    }

                    await mediator.Send(new SalvarCompensacaoAusenciaCommand(compensacaoExcluir));

                    await mediator.Send(new ExcluirNotificacaoCompensacaoAusenciaCommand(compensacaoExcluir.Id));

                    unitOfWork.PersistirTransacao();

                    if (alunosDaCompensacao.Any())
                    {
                        await AtualizarCacheCompensacaoAusenciaTurma(turma.CodigoTurma, periodo.Bimestre, compensacaoExcluir.DisciplinaId, compensacoesAlunos);

                        var listaAlunos = alunosDaCompensacao.Select(a => a.CodigoAluno);

                        await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(listaAlunos,
                            periodo.PeriodoFim, turma.CodigoTurma, compensacaoExcluir.DisciplinaId, periodo.MesesDoPeriodo().ToArray()));
                    }
                }
                catch (Exception)
                {
                    idsComErroAoExcluir.Add(compensacaoExcluir.Id);
                    unitOfWork.Rollback();
                }
            }
            return idsComErroAoExcluir;
        }
        private async Task<PeriodoEscolarDto> BuscaPeriodo(Turma turma, int bimestre)
        {
            var modalidadeTipoCalendario = turma.ModalidadeCodigo.ObterModalidadeTipoCalendario();
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, modalidadeTipoCalendario, turma.Semestre));

            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PermiteCompensacaoForaPeriodo, turma.AnoLetivo));

            PeriodoEscolarDto periodo = null;
            var consulta = await mediator.Send(new ObterPeriodoEscolarListaPorTipoCalendarioQuery(tipoCalendario.Id));
            if (turma.ModalidadeCodigo == Modalidade.EJA)
            {
                if (turma.Semestre == 1)
                    periodo = consulta.Periodos.FirstOrDefault(p => p.Bimestre == bimestre && p.PeriodoInicio < new DateTime(turma.AnoLetivo, 6, 1));
                else
                    periodo = consulta.Periodos.FirstOrDefault(p => p.Bimestre == bimestre && p.PeriodoFim > new DateTime(turma.AnoLetivo, 6, 1));
            }
            else
                periodo = consulta.Periodos.FirstOrDefault(p => p.Bimestre == bimestre);

            if (parametroSistema is not { Ativo: true })
            {
                var turmaEmPeriodoAberto =
                    await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, false,
                        tipoCalendario.Id));

                if (!turmaEmPeriodoAberto)
                    throw new NegocioException($"Período do {bimestre}º Bimestre não está aberto.");
            }

            return periodo;
        }

        private async Task AtualizarCacheCompensacaoAusenciaTurma(string codigoTurma, int bimestre, string componenteCurricularId, List<CompensacaoAusenciaAluno> compensacaoAusenciaAlunosExcluir)
        {
            var valorNomeChave = string.Format(NomeChaveCache.NOME_CHAVE_COMPENSACAO_TURMA_BIMESTRE, codigoTurma, bimestre);

            var cacheCompensacaoTurma = await repositorioCache
                .ObterAsync(valorNomeChave);

            if (string.IsNullOrEmpty(cacheCompensacaoTurma))
                return;

            var compensacoesTurma = JsonConvert
                .DeserializeObject<List<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>(cacheCompensacaoTurma);

            var compensacoesAtualizacao = from ce in compensacaoAusenciaAlunosExcluir
                                          join c in compensacoesTurma.Where(ct => ct.ComponenteCurricularId == componenteCurricularId)
                                          on ce.CodigoAluno equals c.AlunoCodigo
                                          select new { compensacaoAtualizar = c, compensacaoExclusao = ce };

            foreach (var compensacaoAtualizarAtual in compensacoesAtualizacao)
            {
                var saldoCompensacoes = compensacaoAtualizarAtual.compensacaoAtualizar.Compensacoes - compensacaoAtualizarAtual.compensacaoExclusao.QuantidadeFaltasCompensadas;
                if (saldoCompensacoes < 1)
                {
                    compensacoesTurma.Remove(compensacaoAtualizarAtual.compensacaoAtualizar);
                    continue;
                }
                compensacaoAtualizarAtual.compensacaoAtualizar.Compensacoes = saldoCompensacoes;
            }

            if (compensacoesTurma.Any())
            {
                await mediator
                    .Send(new CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand(codigoTurma, bimestre, compensacoesTurma));
                return;
            }


            await repositorioCache.RemoverAsync(valorNomeChave);
        }
    }
}