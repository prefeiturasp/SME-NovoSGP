using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Queries.Turma.ObterTurmaIdPorCodigo
{
    public class ObterTurmaIdPorCodigoQuery : IRequest<long>
    {
        public string TurmaCodigo { get; set; }

        public ObterTurmaIdPorCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }
    }
    public class ObterTurmaIdPorCodigoValidator : AbstractValidator<ObterTurmaIdPorCodigoQuery>
    {

        public ObterTurmaIdPorCodigoValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
