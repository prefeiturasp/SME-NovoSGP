using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseNotaDuplicadoUseCase : IConselhoClasseNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhoClasseNotaDuplicado repositorioConselhoClasseNotaDuplicado;
        private readonly IMediator mediator;

        public ConselhoClasseNotaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                  IRepositorioConselhoClasseNotaDuplicado repositorioConselhoClasseNotaDuplicado,
                                                  IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhoClasseNotaDuplicado = repositorioConselhoClasseNotaDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseNotaDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioConselhoClasseNotaDuplicado.ExcluirTodos();

            var conselhosClasseNotaDuplicados = await repositorioSGP.ObterConselhosClasseNotaDuplicados();

            foreach (var conselhoClasseNotaDuplicado in conselhosClasseNotaDuplicados)
            {
                await repositorioConselhoClasseNotaDuplicado.InserirAsync(conselhoClasseNotaDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaConselhoClasseNotaDuplicado, conselhoClasseNotaDuplicado));
            }

            return true;
        }
    }
}
