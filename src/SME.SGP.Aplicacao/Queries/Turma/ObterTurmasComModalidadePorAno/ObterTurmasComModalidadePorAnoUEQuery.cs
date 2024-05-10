using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorAnoUEQuery : IRequest<IEnumerable<TurmaModalidadeDto>>
    {
        public ObterTurmasComModalidadePorAnoUEQuery(int ano, long ueId)
        {
            Ano = ano;
            UeId = ueId;
        }

        public int Ano { get; }
        public long UeId { get; }
    }

    public class ObterTurmasComModalidadePorAnoQueryValidator : AbstractValidator<ObterTurmasComModalidadePorAnoUEQuery>
    {
        public ObterTurmasComModalidadePorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para consulta de turmas e modalidades");

            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consulta de turmas e modalidades");
        }
    }
}
