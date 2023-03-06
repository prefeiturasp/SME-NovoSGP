using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorIdSemPerfilQuery : IRequest<Dominio.Usuario>
    {
        public ObterUsuarioPorIdSemPerfilQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterUsuarioPorIdSemPerfilQueryValidator : AbstractValidator<ObterUsuarioPorIdSemPerfilQuery>
    {
        public ObterUsuarioPorIdSemPerfilQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O Id do usuário deve ser informado.");
        }
    }
}
