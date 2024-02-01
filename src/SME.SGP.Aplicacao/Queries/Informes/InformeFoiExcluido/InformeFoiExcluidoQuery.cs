using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class InformeFoiExcluidoQuery : IRequest<bool>
    {
        public InformeFoiExcluidoQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class InformeFoiExcluidoQueryValidator : AbstractValidator<InformeFoiExcluidoQuery>
    {
        public InformeFoiExcluidoQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do infomes deve ser informado para verificar a exclusão.");
        }
    }
}
