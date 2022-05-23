using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommand : IRequest
    {
        public SalvarPendenciaDiarioBordoCommand()
        {}

        public string ProfessorRf { get; set; }
        public long PendenciaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AulaId { get; set; }

    }

    public class SalvarPendenciaDiarioBordoCommandValidator : AbstractValidator<SalvarPendenciaDiarioBordoCommand>
    {
        public SalvarPendenciaDiarioBordoCommandValidator()
        {
            RuleForEach(c => c.ProfessorRf)
            .NotEmpty()
            .WithMessage("O código do professor deve ser informa para geração de pendência diário de bordo.");

            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O identificador da pendência deve ser informado para geração de pendência diário de bordo.");

            RuleFor(c => c.ComponenteCurricularId)
            .NotEmpty()
            .WithMessage("O identificador do componente curricular deve ser informado para geração de pendência diário de bordo.");

            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O identificador da aula deve ser informado para geração de pendência diário de bordo.");
        }
    }
}
