using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarComponentesDoProfessorCommandHandler : IRequestHandler<ValidarComponentesDoProfessorCommand, (bool resultado, string mensagem)>
    {
        private readonly IMediator mediator;

        public ValidarComponentesDoProfessorCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<(bool resultado, string mensagem)> Handle(ValidarComponentesDoProfessorCommand request, CancellationToken cancellationToken)
        {
            if (request.Usuario.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = await mediator.Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(request.Usuario.Login));
                if (componentesCurricularesDoProfessorCJ == null || !componentesCurricularesDoProfessorCJ.Any(c => c.TurmaId == request.TurmaCodigo && c.DisciplinaId == request.ComponenteCurricularCodigo ))
                    return (false, "Você não pode criar aulas para essa Turma.");
            }
            else
            {
                var componentesCurricularesDoProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(request.TurmaCodigo, request.Usuario.Login, request.Usuario.PerfilAtual));
                if (componentesCurricularesDoProfessor == null || !componentesCurricularesDoProfessor.Any(c => c.Codigo == request.ComponenteCurricularCodigo))
                    return (false, "Você não pode criar aulas para essa Turma.");

                var usuarioPodePersistirTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(request.ComponenteCurricularCodigo, request.TurmaCodigo, request.Data, request.Usuario));
                if (!usuarioPodePersistirTurmaNaData)
                    return (false, "Você não pode fazer alterações ou inclusões nesta turma, disciplina e data.");
            }

            return (true, string.Empty);
        }
    }
}
