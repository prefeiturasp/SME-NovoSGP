using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase : AbstractUseCase, IExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;

        public ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            var mensagemConsolidacao = new MensagemConsolidacaoMigracaoDto();

            try
            {

                //RepositorioComponenteCurricularConsulta.ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre

                var consolidacoesParaTatar = new List<ConsolidacaoConselhoClasseAlunoMigracaoDto>();

                var fechamentosNotaParaTratar = new List<FechamentoNotaMigracaoDto>();

                foreach (var consolidacaoAluno in consolidacoesParaTatar)
                {
                    var fechamentoNota = fechamentosNotaParaTratar.FirstOrDefault(f => f.Bimestre == consolidacaoAluno.Bimestre
                                                                                      && f.TurmaId == consolidacaoAluno.TurmaId
                                                                                      && f.Bimestre == consolidacaoAluno.Bimestre
                                                                                      && f.AlunoCodigo.Equals(consolidacaoAluno.AlunoCodigo));

                    mensagemConsolidacao = new MensagemConsolidacaoMigracaoDto()
                    {
                        AlunoCodigo = consolidacaoAluno.AlunoCodigo,
                        Bimestre = consolidacaoAluno.Bimestre,
                        TurmaId = consolidacaoAluno.TurmaId,
                        ConsolidacaoId = consolidacaoAluno.ConsolidacaoId,
                        Nota = fechamentoNota.Nota,
                        ConceitoId = fechamentoNota.ConceitoId,
                        DisciplinaId = fechamentoNota.DisciplinaId
                    };

                    var publicarFilaConsolidacaoConselhoClasseAluno = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, mensagemParaPublicar, mensagemRabbit.CodigoCorrelacao, null));
                }
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }

            return true;
        }
    }
}
