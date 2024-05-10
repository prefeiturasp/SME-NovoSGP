using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery : IRequest<QuestaoEncaminhamentoNAAPA>
    {
        public ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQueryValidator : AbstractValidator<ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery>
    {
        public ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Id do encaminhamento NAAPA deve ser informado para a busca das turmas de programa do aluno.");
        }
    }
}
