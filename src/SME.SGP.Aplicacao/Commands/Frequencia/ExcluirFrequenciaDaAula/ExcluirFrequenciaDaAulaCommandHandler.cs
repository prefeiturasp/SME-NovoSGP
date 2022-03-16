using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciaDaAulaCommandHandler : IRequestHandler<ExcluirFrequenciaDaAulaCommand, bool>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IMediator mediator;
        public ExcluirFrequenciaDaAulaCommandHandler(IRepositorioFrequencia repositorioFrequencia,IMediator mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirFrequenciaDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequencia.ExcluirFrequenciaAula(request.AulaId);
            return true;
        }
    }
}
