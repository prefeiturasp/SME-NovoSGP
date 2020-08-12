using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntentocesPorIdQuery : IRequest<CartaIntencoes>
    {
        public long Id { get; set; }

        public ObterCartaIntentocesPorIdQuery(long id)
        {
            Id = id;
        }
    }

    public class ObterCartaIntentocesPorIdValidator : AbstractValidator<ObterCartaIntentocesPorIdQuery>
    {

        public ObterCartaIntentocesPorIdValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O código da carta de intenções deve ser informado.");
        }
    }
}
