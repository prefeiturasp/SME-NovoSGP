using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualUseCase : IMigrarPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public MigrarPlanejamentoAnualUseCase(IMediator mediator,
                                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MigrarPlanejamentoAnualDto dto)
        {
            unitOfWork.IniciarTransacao();

            var retorno = await mediator.Send(new MigrarPlanejamentoAnualCommand(dto));

            unitOfWork.PersistirTransacao();

            return retorno;
        }

    }
}
