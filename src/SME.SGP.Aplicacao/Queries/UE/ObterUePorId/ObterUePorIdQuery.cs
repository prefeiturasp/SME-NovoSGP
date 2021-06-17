using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUePorIdQuery : IRequest<Ue>
    {
        public ObterUePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
    public class ObterUePorIdQueryValidator : AbstractValidator<ObterUePorIdQuery>
    {
        public ObterUePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id devem ser informado para consulta das UEs.");
        }
    }
}
