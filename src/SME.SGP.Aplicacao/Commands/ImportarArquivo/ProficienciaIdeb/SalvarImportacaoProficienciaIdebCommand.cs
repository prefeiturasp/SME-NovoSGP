using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdeb
{
    public class SalvarImportacaoProficienciaIdebCommand : IRequest<bool>
    {
        public SalvarImportacaoProficienciaIdebCommand(ProficienciaIdebDto proficienciaIdeb)
        {
            ProficienciaIdeb = proficienciaIdeb;
        }

        public ProficienciaIdebDto ProficienciaIdeb { get; }

        public class SalvarImportacaoProficienciaIdebCommandValidator : AbstractValidator<SalvarImportacaoProficienciaIdebCommand>
        {
            public SalvarImportacaoProficienciaIdebCommandValidator()
            {
                RuleFor(x => x.ProficienciaIdeb.AnoLetivo)
                    .GreaterThan(0).WithMessage("Ano Letivo inválido");

                RuleFor(x => x.ProficienciaIdeb.CodigoEOLEscola)
                    .NotEmpty().WithMessage("Código EOL da UE inválido")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Código EOL da UE inválido");
            }
        }
    }
}
