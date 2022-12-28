using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterEncaminhamentoNAAPAPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a pesquisa.");
        }
    }
}
