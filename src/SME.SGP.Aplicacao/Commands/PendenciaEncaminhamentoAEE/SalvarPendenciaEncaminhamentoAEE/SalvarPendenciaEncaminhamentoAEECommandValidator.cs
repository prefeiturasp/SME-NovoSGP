using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaEncaminhamentoAEECommandValidator : AbstractValidator<SalvarPendenciaEncaminhamentoAEECommand>
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
