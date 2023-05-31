using MediatR;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQueryHandler : IRequestHandler<ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery, string[]>
    {
        private readonly IMediator mediator;

        public ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string[]> Handle(ObterProfessoresAtribuidosPorCodigosComponentesTerritorioQuery request, CancellationToken cancellationToken)
        {
            var professoresAtreladosComponentes = new List<string>();

            foreach (var componenteAtual in request.CodigosComponentesTerritorio)
            {
                var codigosComponentesTerritorio = await mediator
                    .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(componenteAtual, request.CodigoTurma, null));

                professoresAtreladosComponentes
                    .AddRange(codigosComponentesTerritorio.Select(c => c.professor));
            }

            return professoresAtreladosComponentes
                .Except(new string[] { request.ProfessorDesconsiderado })
                .Distinct()
                .ToArray();
        }
    }
}
