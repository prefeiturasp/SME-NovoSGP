using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.Rotas;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseDuplicadoUseCase : IConselhoClasseDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhoClasseDuplicado repositorioConselhoClasseDuplicado;
        private readonly IMediator mediator;

        public ConselhoClasseDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                              IRepositorioConselhoClasseDuplicado repositorioConselhoClasseDuplicado,
                                              IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhoClasseDuplicado = repositorioConselhoClasseDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioConselhoClasseDuplicado.ExcluirTodos();

            var conselhosClasse = await repositorioSGP.ObterConselhosClasseDuplicados();

            foreach (var conselhoClasse in conselhosClasse)
            {
                await repositorioConselhoClasseDuplicado.InserirAsync(conselhoClasse);
                await mediator.Send(new PublicarFilaCommand(RotasRabbitMetrica.LimpezaConselhoClasseDuplicado, conselhoClasse));
            }

            return true;
        }
    }
}
