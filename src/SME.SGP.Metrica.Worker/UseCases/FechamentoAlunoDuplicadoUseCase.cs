using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoAlunoDuplicadoUseCase : IFechamentoAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoAlunoDuplicado repositorioFechamentoAlunoDuplicado;
        private readonly IMediator mediator;

        public FechamentoAlunoDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                               IRepositorioFechamentoAlunoDuplicado repositorioFechamentoAlunoDuplicado,
                                               IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoAlunoDuplicado = repositorioFechamentoAlunoDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoAlunoDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioFechamentoAlunoDuplicado.ExcluirTodos();

            var ues = await repositorioSGP.ObterUesIds();
            foreach (var ue in ues)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.DuplicacaoFechamentoAlunoUE, new FiltroIdDto(ue)));

            return true;
        }
    }
}
