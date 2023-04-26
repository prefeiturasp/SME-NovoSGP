using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQueryHandler : IRequestHandler<ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQuery, (long codigo, string rf)>
    {
        private readonly IMediator mediator;

        public ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<(long codigo, string rf)> Handle(ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQuery request, CancellationToken cancellationToken)
        {
            var codigoComponenteTerritorioCorrespondente = ((long)0, (string)null);
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery(), cancellationToken);

            if (usuarioLogado.EhProfessor())
            {
                var componentesProfessor = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.Turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual), cancellationToken);

                var componenteCorrespondente = componentesProfessor
                    .FirstOrDefault(cp => cp.Codigo == request.ComponenteCurricularIdBase || cp.CodigoComponenteTerritorioSaber == request.ComponenteCurricularIdBase);

                codigoComponenteTerritorioCorrespondente = (componenteCorrespondente.TerritorioSaber && componenteCorrespondente != null && componenteCorrespondente.Codigo == request.ComponenteCurricularIdBase ? 
                        componenteCorrespondente.CodigoComponenteTerritorioSaber : componenteCorrespondente.Codigo, usuarioLogado.CodigoRf);
            }
            else if (usuarioLogado.EhProfessorCj())
            {
                var professores = await mediator
                    .Send(new ObterProfessoresTitularesPorTurmaIdQuery(request.Turma.Id), cancellationToken);

                var professor = professores.FirstOrDefault(p => p.DisciplinasId.Contains(request.ComponenteCurricularIdBase));
                if (professor != null && !string.IsNullOrWhiteSpace(professor.ProfessorRf))
                {
                    var componentesProfessor = await mediator
                        .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.Turma.CodigoTurma, professor.ProfessorRf, Perfis.PERFIL_PROFESSOR), cancellationToken);

                    var componenteProfessorRelacionado = componentesProfessor
                        .FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber == request.ComponenteCurricularIdBase);

                    if (componenteProfessorRelacionado != null)
                        codigoComponenteTerritorioCorrespondente = (componenteProfessorRelacionado.Codigo, professor.ProfessorRf);
                }
            }

            return codigoComponenteTerritorioCorrespondente;
        }
    }
}
