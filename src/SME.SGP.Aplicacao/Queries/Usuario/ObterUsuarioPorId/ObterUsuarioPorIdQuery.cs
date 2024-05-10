using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdQuery : IRequest<Dominio.Usuario>
    {
        public ObterUsuarioPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterUsuarioPorIdQueryValidator : AbstractValidator<ObterUsuarioPorIdQuery>
    {
        public ObterUsuarioPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O Id do usuário deve ser informado.");
        }
    }
}
