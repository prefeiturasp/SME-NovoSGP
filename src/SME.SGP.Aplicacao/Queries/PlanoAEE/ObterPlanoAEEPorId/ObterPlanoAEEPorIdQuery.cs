using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorIdQuery : IRequest<PlanoAEE>
    {
        public ObterPlanoAEEPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterPlanoAEEPorIdQueryValidator : AbstractValidator<ObterPlanoAEEPorIdQuery>
    {
        public ObterPlanoAEEPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id do plano deve ser informado para consulta de suas informações.");
        }
    }
}
