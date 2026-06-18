using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSrmPaeeColaborativoUseCase : IObterSrmPaeeColaborativoUseCase
    {
        public ObterSrmPaeeColaborativoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private readonly IMediator mediator;
        public async Task<IEnumerable<SrmPaeeColaborativoSgpDto>> Executar(FiltroSrmPaeeColaborativoDto param)
        {
            return await mediator.Send(new ObterDadosSrmPaeeColaborativoEolQuery(param.CodigoAluno));
        }
    }
}