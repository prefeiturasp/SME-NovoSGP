using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoAlunoPorTurmaIdQuery : IRequest<FechamentoAluno>
    {
        public ObterFechamentoAlunoPorTurmaIdQuery(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            AlunoCodigo = alunoCodigo;
        }

        public long FechamentoTurmaDisciplinaId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class ObterFechamentoAlunoPorTurmaIdQueryValidator : AbstractValidator<ObterFechamentoAlunoPorTurmaIdQuery>
    {
        public ObterFechamentoAlunoPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da disciplina");
            RuleFor(a => a.AlunoCodigo)
             .NotEmpty()
             .WithMessage("Necessário informar o Id do aluno");
        }
    }
}
