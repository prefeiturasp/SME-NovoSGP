using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoPendenciasGeraisUseCase : AbstractUseCase, IExecutaVerificacaoPendenciasGeraisUseCase
    {
        public ExecutaVerificacaoPendenciasGeraisUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            if (!await mediator.Send(new ProcessoEstaEmExecucaoQuery(Dominio.TipoProcesso.CriacaoDePendenciasGerais)))
            {
                var processoId = await mediator.Send(new IncluirProcessoEmExecucaoCommand(Dominio.TipoProcesso.CriacaoDePendenciasGerais));
                try
                {
                    await mediator.Send(new VerificarPendenciaAulaDiasNaoLetivosCommand());
                    await mediator.Send(new VerificaPendenciaCalendarioUeCommand());
                    await mediator.Send(new VerificaPendenciaParametroEventoCommand());
                }
                finally
                {
                    await mediator.Send(new RemoverProcessoEmExecucaoPorIdCommand(processoId));
                }
            }

            return true;
        }
    }
}
