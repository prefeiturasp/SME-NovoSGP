using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorCodigoEModalidadeQueryHandler : IRequestHandler<ObterUesComDrePorCodigoEModalidadeQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesComDrePorCodigoEModalidadeQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesComDrePorCodigoEModalidadeQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUesComDrePorDreEModalidade(request.DreCodigo, request.Modalidade);


    }
}
