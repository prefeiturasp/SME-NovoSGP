using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestoesPlanoAEEPorVersaoQuery(long questionarioId, long versaoPlanoId, string turmaCodigo)
        {
            QuestionarioId = questionarioId;
            VersaoPlanoId = versaoPlanoId;
            TurmaCodigo = turmaCodigo;
        }

        public long QuestionarioId { get; }
        public long VersaoPlanoId { get; }
        public string TurmaCodigo { get; }
    }

    public class ObterQuestoesPlanoAEEPorVersaoQueryValidator : AbstractValidator<ObterQuestoesPlanoAEEPorVersaoQuery>
    {
        public ObterQuestoesPlanoAEEPorVersaoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para aplicação das regras de campos por UE");
        }
    }
}
