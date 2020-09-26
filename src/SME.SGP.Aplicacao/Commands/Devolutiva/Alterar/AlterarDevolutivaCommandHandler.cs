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
    public class AlterarDevolutivaCommandHandler : IRequestHandler<AlterarDevolutivaCommand, AuditoriaDto>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public AlterarDevolutivaCommandHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<AuditoriaDto> Handle(AlterarDevolutivaCommand request, CancellationToken cancellationToken)
        {
            await repositorioDevolutiva.SalvarAsync(request.Devolutiva);

            return (AuditoriaDto)request.Devolutiva;
        }
    }
}
