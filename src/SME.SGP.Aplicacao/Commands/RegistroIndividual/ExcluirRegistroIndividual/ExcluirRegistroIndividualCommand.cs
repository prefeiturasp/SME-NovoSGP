using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroIndividualCommand : IRequest<bool>
    {
        public ExcluirRegistroIndividualCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirRegistroIndividualCommandValidator : AbstractValidator<ExcluirRegistroIndividualCommand>
    {
        public ExcluirRegistroIndividualCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("Necessário informar o Id para exclusão do registro individual");
        }
    }
}
