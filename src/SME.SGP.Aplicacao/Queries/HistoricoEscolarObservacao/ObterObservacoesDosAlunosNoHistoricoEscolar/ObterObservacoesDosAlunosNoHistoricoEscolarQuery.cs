using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDosAlunosNoHistoricoEscolarQuery : IRequest<IEnumerable<Dominio.HistoricoEscolarObservacao>>
    {
        public ObterObservacoesDosAlunosNoHistoricoEscolarQuery(string[] codigosAlunos)
        {
            CodigosAlunos = codigosAlunos;
        }

        public string[] CodigosAlunos { get; }
    }

    public class ObterObservacoesDosAlunosNoHistoricoEscolarQueryValidator : AbstractValidator<ObterObservacoesDosAlunosNoHistoricoEscolarQuery>
    {
        public ObterObservacoesDosAlunosNoHistoricoEscolarQueryValidator()
        {
            RuleFor(f => f.CodigosAlunos)
                .NotEmpty()
                .WithMessage("Os Códigos dos Alunos devem ser informados.");
        }
    }
}
