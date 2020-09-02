using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaCommandHandler : IRequestHandler<InserirDevolutivaCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public InserirDevolutivaCommandHandler(IMediator mediator,
                                                IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<AuditoriaDto> Handle(InserirDevolutivaCommand request, CancellationToken cancellationToken)
        {
            Devolutiva devolutiva = MapearParaEntidade(request);

            await repositorioDevolutiva.SalvarAsync(devolutiva);

            return (AuditoriaDto)devolutiva;
        }

        private Devolutiva MapearParaEntidade(InserirDevolutivaCommand request)
            => new Devolutiva()
            {
                CodigoComponenteCurricular = request.CodigoComponenteCurricular,
                PeriodoInicio = request.PeriodoInicio,
                PeriodoFim = request.PeriodoFim,
                Descricao = request.Descricao
            };
    }
}
