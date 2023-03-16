using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DefinirComponenteCurricularParaAulaQueryHandler : IRequestHandler<DefinirComponenteCurricularParaAulaQuery, (long codigoComponente, long? codigoTerritorio)>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public DefinirComponenteCurricularParaAulaQueryHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }
        public async Task<(long codigoComponente, long? codigoTerritorio)> Handle(DefinirComponenteCurricularParaAulaQuery request, CancellationToken cancellationToken)
        {
            var componentesProfessor = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, request.Usuario.Login, request.Usuario.PerfilAtual));

            var componenteCorrespondenteCorrespondente = componentesProfessor
                .FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.Equals(request.CodigoComponenteCurricular));

            if (componenteCorrespondenteCorrespondente != null)
                return (componenteCorrespondenteCorrespondente.Codigo, request.CodigoComponenteCurricular);
            else if (request.Usuario.EhProfessorCj())
            {
                var atribuicoesCjProfessor = await mediator
                    .Send(new ObterAtribuicoesCJAtivasQuery(request.Usuario.CodigoRf, false));

                var atribuicaoDisciplinaCorrespondente = atribuicoesCjProfessor
                    .FirstOrDefault(a => a.DisciplinaId.Equals(request.CodigoComponenteCurricular));

                if (atribuicaoDisciplinaCorrespondente == null)
                {
                    var componentesTurma = await mediator
                        .Send(new ObterComponentesCurricularesPorTurmaCodigoQuery(request.CodigoTurma));

                    var componenteTurmaCorrespondente = componentesTurma
                        .FirstOrDefault(ct => ct.CodigoComponenteCurricular.Equals(request.CodigoComponenteCurricular));

                    if (componenteTurmaCorrespondente != null)
                    {
                        atribuicaoDisciplinaCorrespondente = atribuicoesCjProfessor
                            .FirstOrDefault(a => a.DisciplinaId.Equals(componenteTurmaCorrespondente.CodigoComponenteTerritorioSaber));

                        return (request.CodigoComponenteCurricular, componenteTurmaCorrespondente.CodigoComponenteTerritorioSaber);
                    }
                }               
            }

            return default;
        }
    }
}
