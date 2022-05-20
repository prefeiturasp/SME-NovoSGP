using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDataCriacaoRelatorioPorCodigoQuery: IRequest<DataCriacaoRelatorioDto>
    {
        public Guid CodigoRelatorio { get; set; }

        public ObterDataCriacaoRelatorioPorCodigoQuery(Guid codigoRelatorio)
        {
            CodigoRelatorio = codigoRelatorio;
        }
    }

    public class ObterDataCriacaoRelatorioPorCodigoQueryValidator : AbstractValidator<ObterDataCriacaoRelatorioPorCodigoQuery>
    {
        public ObterDataCriacaoRelatorioPorCodigoQueryValidator()
        {

            RuleFor(c => c.CodigoRelatorio)
            .NotEmpty()
            .WithMessage("O Código do Relatório deve ser informado.");
        }
    }
}
