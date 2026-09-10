using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRelatorioPAPUseCase : AbstractUseCase, ISalvarRelatorioPAPUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public SalvarRelatorioPAPUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ResultadoRelatorioPAPDto> Executar(RelatorioPAPDto relatorioPAPDto)
        {
            unitOfWork.IniciarTransacao();

            try
            {
                var resultado = await mediator.Send(new PersistirRelatorioPAPCommand(relatorioPAPDto));
                unitOfWork.PersistirTransacao();
                return resultado;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
