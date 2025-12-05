using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoQuestionarioAtendimentoNAAPAPorIdQuery : IRequest<SecaoQuestionarioDto>
    {
        public ObterSecaoQuestionarioAtendimentoNAAPAPorIdQuery(long secaoId)
        {
            SecaoId = secaoId;
        }

        public long SecaoId { get; set; }
    }

    public class ObterSecaoQuestionarioAtendimentoNAAPAPorIdQueryValidator : AbstractValidator<ObterSecaoQuestionarioAtendimentoNAAPAPorIdQuery>
    {
        public ObterSecaoQuestionarioAtendimentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.SecaoId)
                .NotEmpty()
                .WithMessage("O id da seção deve ser informado para obter a seção de questionário do atendimento NAAPA.");
        }
    }
}
