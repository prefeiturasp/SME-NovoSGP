using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery : IRequest<long>
    {
        public ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery(string turmaId, string disciplinaId)
        {            
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
        }
                
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }
    public class ObterPendenciaAulaPorTurmaIdDisciplinaIdQueryValidator : AbstractValidator<ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery>
    {
        public ObterPendenciaAulaPorTurmaIdDisciplinaIdQueryValidator()
        {            
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O Id da tuma deve ser informado.");
            RuleFor(c => c.DisciplinaId)
               .NotEmpty()
               .WithMessage("O Id da disciplina deve ser informado.");
        }
    }
}
