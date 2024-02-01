using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseNaoConsolidadoUseCase : IConselhoClasseNaoConsolidadoUseCase
    {
        private readonly IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado;
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IMediator mediator;

        public ConselhoClasseNaoConsolidadoUseCase(IRepositorioConselhoClasseNaoConsolidado repositorioConselhoNaoConsolidado,
                                                   IRepositorioSGPConsulta repositorioSGP,
                                                   IMediator mediator)
        {
            this.repositorioConselhoNaoConsolidado = repositorioConselhoNaoConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoNaoConsolidado));
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioConselhoNaoConsolidado.ExcluirTodos();

            var ues = await repositorioSGP.ObterUesIds();
            foreach (var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.ConselhoClasseNaoConsolidadoUE, new FiltroIdDto(ue)));

            return true;
        }
    }
}
