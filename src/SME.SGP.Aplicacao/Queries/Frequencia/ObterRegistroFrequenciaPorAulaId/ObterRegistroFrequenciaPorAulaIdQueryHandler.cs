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
    public class ObterRegistroFrequenciaPorAulaIdQueryHandler : IRequestHandler<ObterRegistroFrequenciaPorAulaIdQuery, RegistroFrequencia>
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ObterRegistroFrequenciaPorAulaIdQueryHandler(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }

        public async Task<RegistroFrequencia> Handle(ObterRegistroFrequenciaPorAulaIdQuery request, CancellationToken cancellationToken)
            => await repositorioFrequencia.ObterRegistroFrequenciaPorAulaId(request.AulaId);
    }
}
