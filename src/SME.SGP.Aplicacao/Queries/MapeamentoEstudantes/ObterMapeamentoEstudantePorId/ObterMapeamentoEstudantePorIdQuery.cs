using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterMapeamentoEstudantePorIdQuery : IRequest<MapeamentoEstudante>
    {
        public ObterMapeamentoEstudantePorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterMapeamentoEstudantePorIdQueryValidator : AbstractValidator<ObterMapeamentoEstudantePorIdQuery>
    {
        public ObterMapeamentoEstudantePorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do mapeamento de estudante deve ser informado para a pesquisa.");
        }
    }
}
