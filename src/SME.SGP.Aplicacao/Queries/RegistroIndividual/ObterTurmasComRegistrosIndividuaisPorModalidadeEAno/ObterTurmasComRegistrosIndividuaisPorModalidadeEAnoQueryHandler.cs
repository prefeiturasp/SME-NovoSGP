using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQueryHandler : IRequestHandler<ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery, IEnumerable<RegistroIndividualDTO>>
    {
        private readonly IRepositorioRegistroIndividual repositorio;

        public ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQueryHandler(IRepositorioRegistroIndividual repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<RegistroIndividualDTO>> Handle(ObterTurmasComRegistrosIndividuaisPorModalidadeEAnoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterTurmasComRegistrosIndividuaisInfantilEAnoAsync(request.AnoLetivo, request.Modalidades);
    }
}
