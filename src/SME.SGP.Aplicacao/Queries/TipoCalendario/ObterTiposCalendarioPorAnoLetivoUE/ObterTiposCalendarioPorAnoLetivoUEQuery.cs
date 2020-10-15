using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoUEQuery : IRequest<IEnumerable<TipoCalendario>>
    {
        public ObterTiposCalendarioPorAnoLetivoUEQuery(int anoLetivo, string codigoUE)
        {
            AnoLetivo = anoLetivo;
            CodigoUE = codigoUE;
        }

        public int AnoLetivo { get; }
        public string CodigoUE { get; }
    }

    public class ObterTiposCalendarioPorAnoLetivoUEQueryValidator : AbstractValidator<ObterTiposCalendarioPorAnoLetivoUEQuery>
    {
        public ObterTiposCalendarioPorAnoLetivoUEQueryValidator()
        {
            RuleFor(x => x.CodigoUE).NotEmpty().WithMessage("Código UE é obrigatório");
        }
    }
}
