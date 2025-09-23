using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class SalvarImportacaoProficienciaIdepCommand : IRequest<bool>
    {
        public SalvarImportacaoProficienciaIdepCommand(ProficienciaIdepDto proficienciaIdep)
        {
            ProficienciaIdep = proficienciaIdep;
        }

        public ProficienciaIdepDto ProficienciaIdep { get; }

        public class SalvarImportacaoProficienciaIdepCommandValidator : AbstractValidator<SalvarImportacaoProficienciaIdepCommand>
        {
            public SalvarImportacaoProficienciaIdepCommandValidator()
            {
                RuleFor(x => x.ProficienciaIdep.AnoLetivo)
                    .GreaterThan(0).WithMessage("Ano Letivo inválido");

                RuleFor(x => x.ProficienciaIdep.CodigoEOLEscola)
                    .NotEmpty().WithMessage("Código EOL da UE inválido")
                    .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Código EOL da UE inválido");

                RuleFor(x => x.ProficienciaIdep.SerieAno)
                    .GreaterThan(0).WithMessage("Serie/Ano inválido");

                RuleFor(x => x.ProficienciaIdep.Proficiencia)
                    .GreaterThan(0).WithMessage("Proficiencia inválido");
            }
        }
    }
}
