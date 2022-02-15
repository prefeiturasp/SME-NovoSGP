using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorTurmaCodigoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterAlunosAtivosPorTurmaCodigoQuery(string turmaCodigo, DateTime dataAula)
        {
            TurmaCodigo = turmaCodigo;
            DataAula = dataAula;
        }

        public string TurmaCodigo { get; set; }
        public DateTime DataAula { get; set; }
    }
    public class ObterAlunosAtivosPorTurmaCodigoQueryValidator : AbstractValidator<ObterAlunosAtivosPorTurmaCodigoQuery>
    {
        public ObterAlunosAtivosPorTurmaCodigoQueryValidator()
        {

            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.DataAula)
               .NotEmpty()
               .WithMessage("A data da aula deve ser informado.");
        }
    }
}

