using MediatR;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Minio.DataModel;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQueryHandler : IRequestHandler<ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery, (string codigoComponente, string professor)[]>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQueryHandler(IMediator mediator,
                                                                                       IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<(string codigoComponente, string professor)[]> Handle(ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Professor))
            {
                var disciplinasEOL = await servicoEol
                    .ObterDisciplinasPorIdsAgrupadas(new long[] { request.CodigoComponenteBase }, request.CodigoTurma);

                if (disciplinasEOL == null || !disciplinasEOL.Any())
                    return new (string, string)[] { (request.CodigoComponenteBase.ToString(), null) };

                if (disciplinasEOL.All(d => d.CodigoTerritorioSaber == 0))
                {
                    var componentesTurma = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, "Sistema", Perfis.PERFIL_ADMSME));

                    var componentesTurmaCorrespondentes = componentesTurma
                        .Where(ct => ct.CodigoComponenteTerritorioSaber.Equals(request.CodigoComponenteBase));

                    if (componentesTurmaCorrespondentes == null || !componentesTurmaCorrespondentes.Any())
                        return new (string, string)[] { (request.CodigoComponenteBase.ToString(), null) };

                    var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(Convert.ToInt64(request.CodigoTurma)));
                    bool existemProfessoresTitulares = professoresTitulares.Any() && professoresTitulares != null;

                    return componentesTurmaCorrespondentes
                        .Select(ct => (ct.Codigo.ToString(), ct.Professor ?? (existemProfessoresTitulares
                                                                                        ? professoresTitulares.Where(p => p.DisciplinasId.Contains(ct.Codigo)).FirstOrDefault().ProfessorRf
                                                                                        : string.Empty)))
                        .ToArray();
                }

                return disciplinasEOL
                    .Select(d => (d.CodigoTerritorioSaber.ToString(), d.Professor))
                    .ToArray();
            }

            var componentesProfessor = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, request.Professor, Perfis.PERFIL_PROFESSOR));

            var componenteProfessorCorrespondente = componentesProfessor
                .FirstOrDefault(cp => cp.Codigo == request.CodigoComponenteBase ||
                                      cp.CodigoComponenteTerritorioSaber > 0 && cp.CodigoComponenteTerritorioSaber == request.CodigoComponenteBase);

            if (componenteProfessorCorrespondente == null)
                return new (string, string)[] { (request.CodigoComponenteBase.ToString(), request.Professor) };

            return new (string, string)[] { (
                    (componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber > 0 &&
                        componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber == request.CodigoComponenteBase) || componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber == 0 ?
                            componenteProfessorCorrespondente.Codigo.ToString() : componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber.ToString(), request.Professor) };
        }
    }
}
