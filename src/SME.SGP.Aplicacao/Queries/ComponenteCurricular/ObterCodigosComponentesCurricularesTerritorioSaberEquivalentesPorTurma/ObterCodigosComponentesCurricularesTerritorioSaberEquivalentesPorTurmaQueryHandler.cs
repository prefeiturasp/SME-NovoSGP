using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
        
        public ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<(string codigoComponente, string professor)[]> Handle(ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery request, CancellationToken cancellationToken)
        {
            if (FiltrarProfessor(request))
            {
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

            var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.CodigoComponenteBase }));
                
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                return new (string, string)[] { (request.CodigoComponenteBase.ToString(), null) };

            if (PossuiTerritorioSaber(disciplinasEOL))
            {
                var retorno = new List<(string, string)>();

                retorno.AddRange(disciplinasEOL
                    .Select(d => (d.CodigoComponenteCurricularTerritorioSaber.ToString(), d.Professor)));

                retorno.AddRange(new List<(string, string)>() {
                    (request.CodigoComponenteBase.ToString(), disciplinasEOL.FirstOrDefault(d => d.CodigoComponenteCurricular == request.CodigoComponenteBase).Professor) }.Except(retorno));

                return retorno.ToArray();
            }

            var componentesTurma = await mediator
                .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.CodigoTurma, "Sistema", Perfis.PERFIL_ADMSME, checaMotivoDisponibilizacao: false));

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
            bool existemProfessoresTitulares = professoresTitulares != null && professoresTitulares.Any(pt => !string.IsNullOrEmpty(pt.ProfessorRf));

            return componentesTurmaCorrespondentes
                .Select(ct => (ct.Codigo.ToString(), ct.Professor ?? (existemProfessoresTitulares
                    ? professoresTitulares?.Where(p => p.DisciplinasId.Contains(ct.Codigo) || p.DisciplinasId.Contains(ct.CodigoComponenteTerritorioSaber))?.FirstOrDefault()?.ProfessorRf
                    : null)))
                .ToArray();
        }

        private bool PossuiTerritorioSaber(IEnumerable<DisciplinaDto> disciplinasEOL)
            => disciplinasEOL.Any(d => d.CodigoComponenteCurricularTerritorioSaber != 0);

        private bool FiltrarProfessor(ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery request)
            => !string.IsNullOrEmpty(request.Professor);
    }
}
