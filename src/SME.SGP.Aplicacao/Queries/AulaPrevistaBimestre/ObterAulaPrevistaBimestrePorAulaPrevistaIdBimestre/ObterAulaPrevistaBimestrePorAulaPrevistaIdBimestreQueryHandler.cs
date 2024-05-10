using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQueryHandler : IRequestHandler<ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery, IEnumerable<AulaPrevistaBimestre>>
    {
        private readonly IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre;

        public ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQueryHandler(IRepositorioAulaPrevistaBimestre repositorioAulaPrevistaBimestre)
        {
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre ?? throw new ArgumentNullException(nameof(repositorioAulaPrevistaBimestre));
        }
        public Task<IEnumerable<AulaPrevistaBimestre>> Handle(ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestreQuery request, CancellationToken cancellationToken)
         => repositorioAulaPrevistaBimestre.ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestre(request.AulaPrevistaId, request.Bimestres);
    }
}
