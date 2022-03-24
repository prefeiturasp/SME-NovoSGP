using MediatR;
using SME.SGP.Aplicacao.Commands.FechamentoAcompanhamentoTurmas.ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnoAnterior;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase : AbstractUseCase, IExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado;
        private readonly IRepositorioUeConsulta repositorioUeConsulta;
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNotaConsulta;

        public ExecutarMigracaoConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidado repositorioConselhoClasseConsolidado, IRepositorioUeConsulta repositorioUeConsulta,
            IRepositorioFechamentoNotaConsulta repositorioFechamentoNotaConsulta) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidado = repositorioConselhoClasseConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidado));
            this.repositorioUeConsulta = repositorioUeConsulta;
            this.repositorioFechamentoNotaConsulta = repositorioFechamentoNotaConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            //var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidacaoConselhoClasseAlunoDto>();

            //var mensagemConsolidacao = new MensagemConsolidacaoMigracaoDto();

            try
            {
                //Buscar no SGP as UE´s
                var ues = repositorioUeConsulta.ObterTodas();

                foreach (var ue in ues)
                {
                    //Abrir uma fila por escola
                    var ueId = ue.Id.ToString();
                    var turmas = await repositorioUeConsulta.ObterTurmasPorCodigoUe(ueId);

                    foreach (var turma in turmas)
                    {
                        //Abrir uma fila por turma

                        //Buscar as consolidações por turma
                        var consolidacoes = await repositorioConselhoClasseConsolidado.ObterConselhoClasseConsolidadoPorTurmaAsync(turma.Id.ToString());

                        foreach (var consolidacao in consolidacoes)
                        {
                            //Abrir uma fila por aluno
                            //Montar um objeto e chamar command para atualizar a consolidacao

                            var alunoNotas = await repositorioFechamentoNotaConsulta.ObterFechamentoNotaAlunoAsync(consolidacao.TurmaId, consolidacao.AlunoCodigo);

                            var migracaoConsolidacaoTurmaCommand = new ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand()
                            {
                                ConsolidacaoId = consolidacao.ConsolidacaoId,
                                AlunoNotas = alunoNotas,
                            };
                            await mediator.Send(migracaoConsolidacaoTurmaCommand);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                //await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o aluno de codígo : {aluno.CodigoAluno} na fila de consolidação do conselho de classe.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }

            return true;
        }
    }
}
