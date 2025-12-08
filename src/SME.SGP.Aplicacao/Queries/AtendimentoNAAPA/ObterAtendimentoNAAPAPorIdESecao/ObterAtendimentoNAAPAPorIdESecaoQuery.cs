using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAPorIdESecaoQuery : IRequest<EncaminhamentoNAAPA>
    {
        public ObterAtendimentoNAAPAPorIdESecaoQuery(long id, long secaoId)
        {
            Id = id;
            SecaoId = secaoId;
        }

        public long Id { get; }
        public long SecaoId { get; }
    }

    public class ObterAtendimentoNAAPAPorIdESecaoQueryValidator : AbstractValidator<ObterAtendimentoNAAPAPorIdESecaoQuery>
    {
        public ObterAtendimentoNAAPAPorIdESecaoQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do atendimento NAAPA deve ser informado para a pesquisa.");

            RuleFor(c => c.SecaoId)
                .GreaterThan(0)
                .WithMessage("O Id da seção do atendimento NAAPA deve ser informado para a pesquisa.");
        }
    }
}
