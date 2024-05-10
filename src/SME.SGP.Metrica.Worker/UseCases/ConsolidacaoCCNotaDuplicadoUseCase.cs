using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConsolidacaoCCNotaDuplicadoUseCase : IConsolidacaoCCNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConsolidacaoCCNotaDuplicado repositorioDuplicados;
        private readonly IMediator mediator;

        public ConsolidacaoCCNotaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                  IRepositorioConsolidacaoCCNotaDuplicado repositorioDuplicados,
                                                  IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioDuplicados.ExcluirTodos();

            var registrosDuplicados = await repositorioSGP.ObterConsolidacaoCCNotasDuplicados();
            foreach (var registroDuplicado in registrosDuplicados)
            {
                await repositorioDuplicados.InserirAsync(registroDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaConsolidacaoCCNotaDuplicado, registroDuplicado));
            }

            return true;
        }
    }
}
