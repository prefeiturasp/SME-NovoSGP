using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorTurmaCodigoQueryValidator : AbstractValidator<ObterAlunosAtivosPorTurmaCodigoQuery>
    {
        public ObterAlunosAtivosPorTurmaCodigoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
               .NotEmpty()
               .WithMessage("A turma deve ser informada");
        }
    }
}
