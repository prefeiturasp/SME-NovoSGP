using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhoClasseAlunoDuplicadoUseCase : IConselhoClasseAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSgp;
        private readonly IRepositorioConselhoClasseAlunoDuplicado repositorioConselhoClasseAlunoDuplicado;
        private readonly IMediator mediator;

        public ConselhoClasseAlunoDuplicadoUseCase(IRepositorioSGPConsulta repositorioSgp,
                                                   IRepositorioConselhoClasseAlunoDuplicado repositorioConselhoClasseAlunoDuplicado,
                                                   IMediator mediator)
        {
            this.repositorioSgp = repositorioSgp ?? throw new System.ArgumentNullException(nameof(repositorioSgp));
            this.repositorioConselhoClasseAlunoDuplicado = repositorioConselhoClasseAlunoDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseAlunoDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioConselhoClasseAlunoDuplicado.ExcluirTodos();

            var ues = await repositorioSgp.ObterUesIds();
            foreach(var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.DuplicacaoConselhoClasseAlunoUe, new FiltroIdDto(ue)));

            return true;
        }
    }
}
