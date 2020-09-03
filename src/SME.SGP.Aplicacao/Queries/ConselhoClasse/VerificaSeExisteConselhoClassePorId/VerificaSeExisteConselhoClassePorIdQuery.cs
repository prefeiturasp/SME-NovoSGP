using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteConselhoClassePorIdQuery : IRequest<bool>
    {
        public VerificaSeExisteConselhoClassePorIdQuery(long conselhoClasseId)
        {
            ConselhoClasseId = conselhoClasseId;
        }

        public long ConselhoClasseId { get; set; }
    }

    public class VerificaSeExisteConselhoClassePorIdQueryValidator : AbstractValidator<VerificaSeExisteConselhoClassePorIdQuery>
    {
        public VerificaSeExisteConselhoClassePorIdQueryValidator()
        {
            RuleFor(a => a.ConselhoClasseId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id do conselho de classe");
        }
    }
}
