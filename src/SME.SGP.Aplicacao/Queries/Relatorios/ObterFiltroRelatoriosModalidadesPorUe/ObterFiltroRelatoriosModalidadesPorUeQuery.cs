using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; }
    }

    public class ObterFiltroRelatoriosModalidadesPorUeQueryValidator : AbstractValidator<ObterFiltroRelatoriosModalidadesPorUeQuery>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQueryValidator()
        {

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue ser informado.");
        }
    }
}
