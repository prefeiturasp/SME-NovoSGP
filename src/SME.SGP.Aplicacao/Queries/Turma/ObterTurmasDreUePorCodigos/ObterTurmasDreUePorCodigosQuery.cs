using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasDreUePorCodigosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasDreUePorCodigosQuery(string[] codigos)
        {
            Codigos = codigos;
        }

        public string[] Codigos { get; set; }
    }

    public class ObterTurmasDreUePorCodigosQueryValidator : AbstractValidator<ObterTurmasDreUePorCodigosQuery>
    {
        public ObterTurmasDreUePorCodigosQueryValidator()
        {
            RuleFor(a => a.Codigos)
                .NotEmpty()
                .WithMessage("Os códigos de Turmas são necessários para consulta de Turmas");
        }
    }
}
