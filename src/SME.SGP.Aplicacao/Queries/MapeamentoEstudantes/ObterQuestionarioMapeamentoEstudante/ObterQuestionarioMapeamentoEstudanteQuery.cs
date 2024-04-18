using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioMapeamentoEstudanteQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioMapeamentoEstudanteQuery(long questionarioId, long? mapeamentoEstudanteId)
        {
            QuestionarioId = questionarioId;
            MapeamentoEstudanteId = mapeamentoEstudanteId;
        }

        public long QuestionarioId { get; }
        public long? MapeamentoEstudanteId { get; }
    }

    public class ObterQuestionarioMapeamentoEstudanteQueryValidator : AbstractValidator<ObterQuestionarioMapeamentoEstudanteQuery>
    {
        public ObterQuestionarioMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de registro de ação.");
        }
    }
}
