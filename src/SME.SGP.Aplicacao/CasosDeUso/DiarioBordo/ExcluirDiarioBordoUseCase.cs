using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoUseCase : IExcluirDiarioBordoUseCase
    {
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;

        public ExcluirDiarioBordoUseCase(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(long diarioBordoId)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var retorno = await mediator.Send(new ExcluirDiarioBordoCommand(diarioBordoId));
                unitOfWork.PersistirTransacao();
                return retorno;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
