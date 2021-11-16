using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAulaUseCase : AbstractUseCase, ISalvarPlanoAulaUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public SalvarPlanoAulaUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PlanoAulaDto> Executar(PlanoAulaDto planoAulaDto)
        {
            return await mediator.Send(new SalvarPlanoAulaCommand(planoAulaDto));
        }
    }
}
