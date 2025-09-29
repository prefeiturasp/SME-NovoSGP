using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Idep
{
    public class SalvarImportacaoArquivoIdepCommand : IRequest<Dominio.Idep>
    {
        public SalvarImportacaoArquivoIdepCommand(ArquivoIdepDto arquivoIdep)
        {
            ArquivoIdep = arquivoIdep;
        }
        public ArquivoIdepDto ArquivoIdep { get; }
    }

    public class SalvarImportacaoArquivoIdebCommandCommandValidator : AbstractValidator<SalvarImportacaoArquivoIdepCommand>
    {
        public SalvarImportacaoArquivoIdebCommandCommandValidator()
        {
            RuleFor(x => x.ArquivoIdep.AnoLetivo)
                .GreaterThan(0).WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoIdep.CodigoEOLEscola)
                .NotEmpty().WithMessage("Código EOL da UE inválido")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Código EOL da UE inválido");

            RuleFor(x => x.ArquivoIdep.SerieAno)
                .GreaterThan(0).WithMessage("Serie/Ano inválido");

            RuleFor(x => x.ArquivoIdep.Nota)
                .GreaterThan(0).WithMessage("Ano Letivo inválido");
        }
    }
}
