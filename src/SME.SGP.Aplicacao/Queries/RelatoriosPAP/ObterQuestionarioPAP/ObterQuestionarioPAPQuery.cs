using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPQuery : IRequest<IEnumerable<QuestaoDto>>
    {
        public ObterQuestionarioPAPQuery(string codigoTurma, string codigoAluno, PeriodoRelatorioPAP periodoPAP, long questionarioId, long? papSecaoId)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            PeriodoRelatorio = periodoPAP;
            QuestionarioId = questionarioId;
            PAPSecaoId = papSecaoId;
        }

        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public PeriodoRelatorioPAP PeriodoRelatorio { get; set; }
        public long QuestionarioId { get; set; } 
        public long? PAPSecaoId { get; set; }
    }

    public class ObterQuestionarioPAPQueryValidator : AbstractValidator<ObterQuestionarioPAPQuery>
    {
        public ObterQuestionarioPAPQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

            RuleFor(x => x.PeriodoRelatorio)
                .NotNull()
                .WithMessage("O período do relatório pap deve ser informado.");

            RuleFor(x => x.QuestionarioId)
                .NotEmpty()
                .WithMessage("O id do questionário do aluno deve ser informado.");
        }
    }
}
