using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseAlunoUeDuplicadoUseCase : IConselhoClasseAlunoUeDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhoClasseAlunoDuplicado repositorioConselhoClasseAlunoDuplicado;
        private readonly IMediator mediator;

        public ConselhoClasseAlunoUeDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                     IRepositorioConselhoClasseAlunoDuplicado repositorioConselhoClasseAlunoDuplicado,
                                                     IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhoClasseAlunoDuplicado = repositorioConselhoClasseAlunoDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseAlunoDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var conselhosClasseAlunoDuplicados = await repositorioSGP.ObterConselhosClasseAlunoDuplicados(ue.Id);

            foreach(var conselhoClasseAlunoDuplicado in conselhosClasseAlunoDuplicados)
            {
                await repositorioConselhoClasseAlunoDuplicado.InserirAsync(conselhoClasseAlunoDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaConselhoClasseAlunoDuplicado, conselhoClasseAlunoDuplicado));
            }

            return true;
        }
    }
}
