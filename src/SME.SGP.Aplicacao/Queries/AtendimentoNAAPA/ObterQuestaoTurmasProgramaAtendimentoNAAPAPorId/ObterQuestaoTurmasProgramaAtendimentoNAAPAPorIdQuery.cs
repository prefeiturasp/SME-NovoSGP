using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQuery : IRequest<QuestaoEncaminhamentoNAAPA>
    {
        public ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQuery>
    {
        public ObterQuestaoTurmasProgramaAtendimentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do atendimento NAAPA deve ser informado para a busca das turmas de programa do aluno.");
        }
    }
}
