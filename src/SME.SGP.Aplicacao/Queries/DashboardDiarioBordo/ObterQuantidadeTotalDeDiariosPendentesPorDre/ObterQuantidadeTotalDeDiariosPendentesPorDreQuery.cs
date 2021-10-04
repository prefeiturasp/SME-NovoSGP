using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPendentesPorDreQuery : IRequest<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>>
    {
        public ObterQuantidadeTotalDeDiariosPendentesPorDreQuery(int anoLetivo, string ano = "")
        {
            AnoLetivo = anoLetivo;
            Ano = ano;
        }

        public int AnoLetivo { get; }
        public string Ano { get; }
    }

    public class ObterQuantidadeTotalDeDiariosPendentesPorDreQueryValidator : AbstractValidator<ObterQuantidadeTotalDeDiariosPendentesPorDreQuery>
    {
        public ObterQuantidadeTotalDeDiariosPendentesPorDreQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta dos diarios de bordos pendentes por DRE");
        }
    }
}
