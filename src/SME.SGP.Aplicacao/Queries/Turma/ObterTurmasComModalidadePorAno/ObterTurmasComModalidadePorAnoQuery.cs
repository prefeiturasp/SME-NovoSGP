using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorAnoQuery : IRequest<IEnumerable<TurmaModalidadeDto>>
    {
        public ObterTurmasComModalidadePorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class ObterTurmasComModalidadePorAnoQueryValidator : AbstractValidator<ObterTurmasComModalidadePorAnoQuery>
    {
        public ObterTurmasComModalidadePorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para consulta de turmas e modalidades");
        }
    }
}
