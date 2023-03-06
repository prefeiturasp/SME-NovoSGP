using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioRfPorIdQuery : IRequest<string>
    {
        public ObterUsuarioRfPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterUsuarioRfPorIdQueryValidator : AbstractValidator<ObterUsuarioRfPorIdQuery>
    {
        public ObterUsuarioRfPorIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O id do usuário deve ser informado para pesquisa do RF");
        }
    }
}
