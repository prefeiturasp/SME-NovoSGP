using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorCodigoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorCodigoQuery() { }
        public ObterTurmasPorCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterTurmasPorCodigoValidator : AbstractValidator<ObterTurmaPorCodigoQuery>
    {
        public ObterTurmasPorCodigoValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
