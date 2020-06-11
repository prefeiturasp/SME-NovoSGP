using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{


    public class ReceberRelatorioProntoUseCase : IReceberRelatorioProntoUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ReceberRelatorioProntoUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            unitOfWork.IniciarTransacao();

            var receberRelatorioProntoCommand = mensagemRabbit.ObterObjetoFiltro<ReceberRelatorioProntoCommand>();
            await mediator.Send(receberRelatorioProntoCommand);

            unitOfWork.PersistirTransacao();
            return await Task.FromResult(true);
        }
    }
}
