using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Idep
{
    public class SalvarImportacaoArquivoIdepCommand : IRequest<ArquivoIdep>
    {
        public SalvarImportacaoArquivoIdepCommand(ArquivoIdepDto arquivoIdep)
        {
            ArquivoIdep = arquivoIdep;
        }
        public ArquivoIdepDto ArquivoIdep { get; }
    }

    public class ImportarArquivoIdepCommandValidator : AbstractValidator<SalvarImportacaoArquivoIdepCommand>
    {
        public ImportarArquivoIdepCommandValidator()
        {
            RuleFor(x => x.ArquivoIdep.AnoLetivo)
                .NotNull().WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoIdep.CodigoEOLEscola)
                .NotNull().WithMessage("Código EOL da UE inválido");

            RuleFor(x => x.ArquivoIdep.SerieAno)
                .NotNull().WithMessage("Série/Ano inválido");

            RuleFor(x => x.ArquivoIdep.Nota)
                .NotNull().WithMessage("Nota inválida");
        }
    }
}
