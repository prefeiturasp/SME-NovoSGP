using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaProfessorCommand : IRequest<long>
    {
        public SalvarPendenciaProfessorCommand(long pendenciaId, long turmaId, long componenteCurricularId, string professorRf, long? periodoEscolarId)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
            ProfessorRf = professorRf;
        }

        public long PendenciaId { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public string ProfessorRf { get; set; }
    }

    public class SalvarPendenciaProfessorCommandValidator : AbstractValidator<SalvarPendenciaProfessorCommand>
    {
        public SalvarPendenciaProfessorCommandValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.ProfessorRf)
               .NotEmpty()
               .WithMessage("O RF do professor deve ser informado para geração da pendência do professor.");
        }
    }
}
