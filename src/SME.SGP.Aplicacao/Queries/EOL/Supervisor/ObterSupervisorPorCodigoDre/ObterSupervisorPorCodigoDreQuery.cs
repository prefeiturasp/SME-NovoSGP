using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisorPorCodigoDreQuery : IRequest<IEnumerable<SupervisoresRetornoDto>>
    {
        public ObterSupervisorPorCodigoDreQuery(string[] supervisorIds, string codigoDre)
        {
            SupervisorIds = supervisorIds;
            CodigoDre = codigoDre;
        }
        public string[] SupervisorIds { get; set; }

        public string CodigoDre { get; set; }
    }

    public class ObterSupervisorPorCodigoDreQueryValidator : AbstractValidator<ObterSupervisorPorCodigoDreQuery>
    {
        public ObterSupervisorPorCodigoDreQueryValidator()
        {
            RuleFor(x => x.SupervisorIds)
                .NotEmpty().WithMessage("O código do supervisor precisa ser informado para obter um supervisor por código.");

            RuleFor(x => x.CodigoDre)
                .NotEmpty().WithMessage("O código da DRE precisa ser informado para obter um supervisor por código.");
        }
    }
}
