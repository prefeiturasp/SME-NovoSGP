using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPorQuestionarioPorIdQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestoesPorQuestionarioPorIdQuery(long questionarioId, ObterRespostasFunc obterRespostas = null)
        {
            QuestionarioId = questionarioId;
            ObterRespostas = obterRespostas;
        }

        public long QuestionarioId { get; }
        public ObterRespostasFunc ObterRespostas { get; }
    }

    public class ObterQuestoesPorQuestionarioPorIdQueryValidator : AbstractValidator<ObterQuestoesPorQuestionarioPorIdQuery>
    {
        public ObterQuestoesPorQuestionarioPorIdQueryValidator()
        {
            RuleFor(a => a.QuestionarioId)
                .NotEmpty()
                .WithMessage("O id do questionário deve ser informado para retorno de suas questões.");
        }
    }
}
