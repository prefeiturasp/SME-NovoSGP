using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAPorIdESecaoQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterEncaminhamentoNAAPAPorIdESecaoQuery(long id, long secaoId)
        {
            Id = id;
            SecaoId = secaoId;
        }

        public long Id { get; }
        public long SecaoId { get; }
    }

    public class ObterEncaminhamentoNAAPAPorIdESecaoQueryValidator : AbstractValidator<ObterEncaminhamentoNAAPAPorIdESecaoQuery>
    {
        public ObterEncaminhamentoNAAPAPorIdESecaoQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a pesquisa.");

            RuleFor(c => c.SecaoId)
                .GreaterThan(0)
                .WithMessage("O Id da seção do encaminhamento NAAPA deve ser informado para a pesquisa.");
        }
    }
}
