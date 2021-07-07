using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaModalidadesPorCodigosQuery : IRequest<IEnumerable<TurmaModalidadeCodigoDto>>
    {
        public ObterTurmaModalidadesPorCodigosQuery(string[] turmasCodigo)
        {
            this.TurmasCodigo = turmasCodigo;
        }

        public string[] TurmasCodigo { get; set; }
    }
    public class ObterTurmaModalidadesPorCodigosQueryValidator : AbstractValidator<ObterTurmaModalidadesPorCodigosQuery>
    {
        public ObterTurmaModalidadesPorCodigosQueryValidator()
        {
            RuleFor(a => a.TurmasCodigo)
                   .Must(x => x == null || x.Length > 0)
                   .WithMessage("Deve ser informado 1 ou mais turmas.");
        }
    }
}
