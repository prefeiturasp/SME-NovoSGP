using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaGeralUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaGeralUseCase
    {
        public ExecutarConsolidacaoTurmaGeralUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turmasEModalidadesParaConsolidar = await mediator.Send(new ObterTurmasConsolidacaoFechamentoGeralQuery());

            var guidParaCorrelacao = Guid.NewGuid();

            foreach (var turmaEModalidadesParaConsolidar in turmasEModalidadesParaConsolidar)
            {
                var mensagem = new ConsolidacaoTurmaDto() { TurmaId = turmaEModalidadesParaConsolidar.TurmaId };

                if (turmaEModalidadesParaConsolidar.Modalidade == Modalidade.EJA)
                {
                    await PublicaNaFila(mensagem, 1, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 2, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 0, guidParaCorrelacao);
                }
                else
                {
                    await PublicaNaFila(mensagem, 1, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 2, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 3, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 4, guidParaCorrelacao);
                    await PublicaNaFila(mensagem, 0, guidParaCorrelacao);
                }
            }

            return true;
        }
        private async Task PublicaNaFila(ConsolidacaoTurmaDto mensagem, int bimestre, Guid codigoCorrelacao)
        {
            mensagem.Bimestre = bimestre;
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaSync, mensagem, codigoCorrelacao, null));
        }
    }
}

