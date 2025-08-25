using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdebCommand : IRequest<ArquivoIdeb>
    {
        public SalvarImportacaoArquivoIdebCommand(ArquivoIdebDto arquivoIdeb)
        {
            ArquivoIdeb = arquivoIdeb;
        }
        public ArquivoIdebDto ArquivoIdeb { get; }
    }

    public class ImportarArquivoIdebCommandValidator : AbstractValidator<SalvarImportacaoArquivoIdebCommand>
    {
        public ImportarArquivoIdebCommandValidator()
        {
            RuleFor(x => x.ArquivoIdeb.AnoLetivo)
                .NotNull().WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoIdeb.CodigoEOLEscola)
                .NotNull().WithMessage("Código EOL da UE inválido");

            RuleFor(x => x.ArquivoIdeb.SerieAno)
                .NotNull().WithMessage("Série/Ano inválido");

            RuleFor(x => x.ArquivoIdeb.Nota)
                .NotNull().WithMessage("Nota inválida");
        }
    }
}
