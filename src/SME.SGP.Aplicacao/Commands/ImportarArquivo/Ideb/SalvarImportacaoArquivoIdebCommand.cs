using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdebCommand : IRequest<Dominio.Entidades.Ideb>
    {
        public SalvarImportacaoArquivoIdebCommand(ArquivoIdebDto arquivoIdeb)
        {
            ArquivoIdeb = arquivoIdeb;
        }
        public ArquivoIdebDto ArquivoIdeb { get; }
    }

    public class SalvarImportacaoArquivoIdebCommandValidator : AbstractValidator<SalvarImportacaoArquivoIdebCommand>
    {
        public SalvarImportacaoArquivoIdebCommandValidator()
        {
            RuleFor(x => x.ArquivoIdeb.AnoLetivo)
                .GreaterThan(0).WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoIdeb.CodigoEOLEscola)
                .NotEmpty().WithMessage("Código EOL da UE inválido")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Código EOL da UE inválido");

            RuleFor(x => x.ArquivoIdeb.SerieAno)
                .GreaterThan(0).WithMessage("Serie/Ano inválido");

            RuleFor(x => x.ArquivoIdeb.Nota)
                .GreaterThan(0).WithMessage("Ano Letivo inválido");
        }
    }
}
