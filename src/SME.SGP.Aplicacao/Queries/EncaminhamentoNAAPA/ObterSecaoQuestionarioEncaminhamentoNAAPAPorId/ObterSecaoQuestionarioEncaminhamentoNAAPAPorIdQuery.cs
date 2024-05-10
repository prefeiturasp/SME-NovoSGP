using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery : IRequest<SecaoQuestionarioDto>
    {
        public ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery(long secaoId)
        {
            SecaoId = secaoId;
        }

        public long SecaoId { get; set; }
    }

    public class ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.SecaoId)
                .NotEmpty()
                .WithMessage("O id da seção deve ser informada para obter a seção de questionário do encaminhamento NAAPA.");
        }
    }
}
