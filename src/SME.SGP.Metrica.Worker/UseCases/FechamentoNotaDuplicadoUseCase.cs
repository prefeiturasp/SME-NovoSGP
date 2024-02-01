using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentoNotaDuplicadoUseCase : IFechamentoNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentoNotaDuplicado repositorioFechamentoNotaDuplicado;
        private readonly IMediator mediator;

        public FechamentoNotaDuplicadoUseCase(IRepositorioSGPConsulta repositorioSGP,
                                              IRepositorioFechamentoNotaDuplicado repositorioFechamentoNotaDuplicado,
                                              IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentoNotaDuplicado = repositorioFechamentoNotaDuplicado ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoNotaDuplicado));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await repositorioFechamentoNotaDuplicado.ExcluirTodos();
            var turmas = await repositorioSGP.ObterTurmasIds(new int[] { (int)Modalidade.EJA, (int)Modalidade.Fundamental, (int)Modalidade.Medio });

            foreach (var turma in turmas)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.DuplicacaoFechamentoNotaTurma, new FiltroIdDto(turma)));

            return true;
        }
    }
}
