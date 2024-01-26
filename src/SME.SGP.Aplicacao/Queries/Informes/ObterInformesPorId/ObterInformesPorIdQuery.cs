using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterInformesPorIdQuery : IRequest<Informativo>
    {
        public ObterInformesPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class ObterInformesPorIdQueryValidator : AbstractValidator<ObterInformesPorIdQuery>
    {
        public ObterInformesPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do infomes deve ser informado para a pesquisa.");
        }
    }
}