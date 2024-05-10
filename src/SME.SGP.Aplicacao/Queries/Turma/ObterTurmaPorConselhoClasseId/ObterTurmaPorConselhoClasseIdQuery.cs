using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorConselhoClasseIdQuery : IRequest<string>
    {
        public ObterTurmaPorConselhoClasseIdQuery() { }
        public ObterTurmaPorConselhoClasseIdQuery(long conselhoClasseId)
        {
            ConselhoClasseId = conselhoClasseId;
        }

        public long ConselhoClasseId { get; set; }
    }

    public class ObterTurmaPorCoselhoClasseIdQueryValidator : AbstractValidator<ObterTurmaPorConselhoClasseIdQuery>
    {

        public ObterTurmaPorCoselhoClasseIdQueryValidator()
        {
            RuleFor(c => c.ConselhoClasseId)
                .NotEmpty()
                .WithMessage("O conselho de classe deve ser informado.");
        }
    }
}
