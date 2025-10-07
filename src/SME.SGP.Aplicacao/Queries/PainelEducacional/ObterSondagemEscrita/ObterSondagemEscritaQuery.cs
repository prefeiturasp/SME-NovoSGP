using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterSondagemEscrita
{
    public class ObterSondagemEscritaQuery : IRequest<IEnumerable<SondagemEscritaDto>>
    {
        public ObterSondagemEscritaQuery(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            SerieAno = serieAno;
        }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public int SerieAno { get; set; }
    }

    public class ObterSondagemEscritaQueryValidator : AbstractValidator<ObterSondagemEscritaQuery>
    {
        public ObterSondagemEscritaQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Bimestre)
           .NotEmpty()
           .WithMessage("O bimestre deve ser informado.");
        }
    }
}
