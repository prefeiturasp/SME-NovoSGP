using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoPorAulaIdUseCase : AbstractUseCase, IExcluirDiarioBordoPorAulaIdUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        
        public ExcluirDiarioBordoPorAulaIdUseCase(IMediator mediator,
            IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            unitOfWork.IniciarTransacao();
            try
            {
                await mediator.Send(new ExcluirDiarioBordoDaAulaCommand(filtro.Id));
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
            
            return true;
        }
    }
}
