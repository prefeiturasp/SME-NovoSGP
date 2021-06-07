using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery : IRequest<long>
    {
        public ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery(string turmaId, string disciplinaId, string professorRf, TipoPendencia tipoPendencia)
        {            
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            ProfessorRf = professorRf;
            TipoPendencia = tipoPendencia;
        }
                
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public string ProfessorRf { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
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
               .WithMessage("O Id do componente curricular deve ser informado.");
        }
    }
}
