using FluentValidation;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioConselhoClasseAtaFinalDto
    {
        public List<string> TurmasCodigos { get; set; }
        public TipoFormatoRelatorio TipoFormatoRelatorio { get; set; }
        public AtaFinalTipoVisualizacao? Visualizacao { get; set; }
        public int AnoLetivo { get;set; }
    }


    public class FiltroRelatorioConselhoClasseAtaFinalDtoValidator : AbstractValidator<FiltroRelatorioConselhoClasseAtaFinalDto>
    {
        public FiltroRelatorioConselhoClasseAtaFinalDtoValidator()
        {
            RuleFor(c => c.TipoFormatoRelatorio)
                .IsInEnum()
                .WithMessage("O tipo de relatório da ata de conselho deve ser informado.");

            RuleFor(c => c.TurmasCodigos)
            .NotEmpty()
            .WithMessage("A lista de turmas deve ser informada.");
        }
    }

}
