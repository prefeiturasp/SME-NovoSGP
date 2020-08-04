using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery :  IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public string CodigoUe { get; }
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }
    }
    //public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryValidator : AbstractValidator<ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery>
    //{
    //    public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryValidator()
    //    {

    //        RuleFor(c => c.CodigoUe)
    //        .NotEmpty()
    //        .WithMessage("O código da ue ser informado.");
    //    }
    //}
}
