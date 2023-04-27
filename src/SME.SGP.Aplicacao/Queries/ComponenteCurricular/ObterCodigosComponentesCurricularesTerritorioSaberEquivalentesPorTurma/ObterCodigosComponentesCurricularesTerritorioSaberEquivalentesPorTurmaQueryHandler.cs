using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
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

                    long turmaid = request.TurmaId > 0 ? request.TurmaId : 0;
                    if (request.TurmaId == 0)
                    {
                        var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.CodigoTurma));
                        turmaid = turma.Id;
                    }

                    var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turmaid));
                    bool existemProfessoresTitulares = professoresTitulares.Any() && professoresTitulares != null;

                    return componentesTurmaCorrespondentes
                        .Select(ct => (ct.Codigo.ToString(), ct.Professor ?? (existemProfessoresTitulares
                            ? professoresTitulares?.Where(p => p.DisciplinasId.Contains(ct.Codigo) || p.DisciplinasId.Contains(ct.CodigoComponenteTerritorioSaber))?.FirstOrDefault()?.ProfessorRf
                            : string.Empty)))
                        .ToArray();
                }

                var retorno = new List<(string, string)>();

                retorno.AddRange(disciplinasEOL
                    .Select(d => (d.CodigoTerritorioSaber.ToString(), d.Professor)));

                retorno.AddRange(new List<(string, string)>() {
                        (request.CodigoComponenteBase.ToString(), disciplinasEOL.FirstOrDefault(d => d.CodigoComponenteCurricular == request.CodigoComponenteBase).Professor) }.Except(retorno));

                return retorno.ToArray();
            }

            var componentesProfessor = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, request.Professor, Perfis.PERFIL_PROFESSOR));

            var componenteProfessorCorrespondente = componentesProfessor
                .FirstOrDefault(cp => cp.Codigo == request.CodigoComponenteBase ||
                                      cp.CodigoComponenteTerritorioSaber > 0 && cp.CodigoComponenteTerritorioSaber == request.CodigoComponenteBase);

            if (componenteProfessorCorrespondente != null && !componenteProfessorCorrespondente.TerritorioSaber)
                return new (string, string)[] { (request.CodigoComponenteBase.ToString(), null) };

            if (componenteProfessorCorrespondente == null)
                return new (string, string)[] { (request.CodigoComponenteBase.ToString(), request.Professor) };

            return new (string, string)[] { (
                    (componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber > 0 &&
                        componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber == request.CodigoComponenteBase) || componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber == 0 ?
                            componenteProfessorCorrespondente.Codigo.ToString() : componenteProfessorCorrespondente.CodigoComponenteTerritorioSaber.ToString(), request.Professor) };
        }
    }
}
