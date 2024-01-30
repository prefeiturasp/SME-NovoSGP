using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoNotaDuplicadoTurmaUseCase : IFechamentoNotaDuplicadoTurmaUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoNotaDuplicado repositorioFechamentoNotaDuplicado;
        private readonly IMediator mediator;

        public FechamentoNotaDuplicadoTurmaUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                   IRepositorioFechamentoNotaDuplicado repositorioFechamentoNotaDuplicado,
                                                   IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoNotaDuplicado = repositorioFechamentoNotaDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoNotaDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turma = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var fechamentosNotaDuplicados = await repositorioSGP.ObterFechamentosNotaDuplicados(turma.Id);
            foreach(var fechamentoNotaDuplicado in fechamentosNotaDuplicados)
            {
                await repositorioFechamentoNotaDuplicado.InserirAsync(fechamentoNotaDuplicado);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.LimpezaFechamentoNotaDuplicado, fechamentoNotaDuplicado));
            }

            return true;
        }
    }
}
