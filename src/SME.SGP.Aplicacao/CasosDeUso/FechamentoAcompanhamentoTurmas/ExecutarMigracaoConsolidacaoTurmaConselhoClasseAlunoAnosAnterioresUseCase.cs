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
                //Buscar no SGP as UE´s
                var ues = new List<Ue>();

                foreach (var ue in ues)
                {
                    //Abrir uma fila por escola
                    var turmas = new List<Turma>();
                    foreach (var turma in turmas)
                    {
                        //Abrir uma fila por turma

                        //Buscar as consolidações por turma
                        //RepositorioComponenteCurricularConsulta.ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre
                        var consolidacoes = new List<ConsolidacaoConselhoClasseAlunoMigracaoDto>();

                        foreach (var consolidacao in consolidacoes)
                        {
                            //Abrir uma fila por aluno
                            //Montar um objeto e chamar command para atualizar a consolidacao
                        }
                    }
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
