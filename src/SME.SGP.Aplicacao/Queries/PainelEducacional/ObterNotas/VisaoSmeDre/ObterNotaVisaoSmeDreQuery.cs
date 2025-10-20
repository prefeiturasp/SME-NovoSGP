using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoSmeDreQuery : IRequest<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>
    {
        public ObterNotaVisaoSmeDreQuery(string codigoDre, int anoLetivo, int bimestre, string anoTurma)
        {
            CodigoDre = codigoDre;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            AnoTurma = anoTurma;
        }
        public string CodigoDre { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public string AnoTurma { get; set; }
    }

    public class ObterNotaVisaoSmeDreQueryValidator : AbstractValidator<ObterNotaVisaoSmeDreQuery>
    {
        public ObterNotaVisaoSmeDreQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Bimestre)
           .NotEmpty()
           .WithMessage("O bimestre deve ser informado.");
        }
    }
}
