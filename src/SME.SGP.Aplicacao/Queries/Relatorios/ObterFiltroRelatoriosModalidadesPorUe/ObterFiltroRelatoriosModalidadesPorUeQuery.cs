using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQuery(string codigoUe, int anoLetivo, bool consideraHistorico)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            ConsideraHistorico = consideraHistorico;
        }

        public string CodigoUe { get; }

        public int AnoLetivo { get; set; }

        public bool ConsideraHistorico { get; set; }
    }

    public class ObterFiltroRelatoriosModalidadesPorUeQueryValidator : AbstractValidator<ObterFiltroRelatoriosModalidadesPorUeQuery>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
