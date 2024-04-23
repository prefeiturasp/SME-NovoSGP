using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery : IRequest<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>>
    {
        public ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery(long questionarioId, long turmaId, string codigoAluno, int bimestre)
        {
            QuestionarioId = questionarioId;
            TurmaId = turmaId;
            CodigoAluno = codigoAluno;
            Bimestre = bimestre;
        }

        public long QuestionarioId { get; }
        public long TurmaId { get; }
        public string CodigoAluno { get; }
        public int Bimestre { get; }
    }

    public class ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQueryValidator : AbstractValidator<ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQuery>
    {
        public ObterRespostasAtualizadasQuestionarioMapeamentoEstudanteQueryValidator()
        {
            RuleFor(c => c.QuestionarioId)
            .NotEmpty()
            .WithMessage("O ID do questionário deve ser informado para consulta do questionário de registro de ação.");
        }
    }
}
