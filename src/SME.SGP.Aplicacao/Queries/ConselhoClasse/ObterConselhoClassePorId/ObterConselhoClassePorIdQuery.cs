using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorIdQuery : IRequest<ConselhoClasse>
    {
        public ObterConselhoClassePorIdQuery(long conselhoClasseId)
        {
            ConselhoClasseId = conselhoClasseId;
        }

        public long ConselhoClasseId { get; set; }
    }

    public class ObterConselhoClassePorIdQueryValidator : AbstractValidator<ObterConselhoClassePorIdQuery>
    {

        public ObterConselhoClassePorIdQueryValidator()
        {
            RuleFor(c => c.ConselhoClasseId)
                .NotEmpty()
                .WithMessage("O conselho de classe deve ser informado.");
        }
    }
}