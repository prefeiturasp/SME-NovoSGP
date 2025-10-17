using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre
{
    public class ObterNotaVisaoSmeDreQuery : IRequest<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>
    {
        public ObterNotaVisaoSmeDreQuery(string codigoDre, int anoLetivo, int bimestre, int anoTurma)
        {
            CodigoDre = codigoDre;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            AnoTurma = anoTurma;
        }
        public string CodigoDre { get; set; }
        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public int AnoTurma { get; set; }
    }

    public class ObterSondagemEscritaQueryValidator : AbstractValidator<ObterNotaVisaoSmeDreQuery>
    {
        public ObterSondagemEscritaQueryValidator()
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
