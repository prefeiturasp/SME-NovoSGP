using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.ImportarArquivo;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Alfabetizacao
{
    public class SalvarImportacaoArquivoAlfabetizacaoCommand : IRequest<Dominio.Alfabetizacao>
    {
        public SalvarImportacaoArquivoAlfabetizacaoCommand(ArquivoAlfabetizacaoDto arquivoAlfabetizacao)
        {
            ArquivoAlfabetizacao = arquivoAlfabetizacao;
        }

        public ArquivoAlfabetizacaoDto ArquivoAlfabetizacao { get; }
    }

    public class SalvarImportacaoArquivoAlfabetizacaoCommandValidator : AbstractValidator<SalvarImportacaoArquivoAlfabetizacaoCommand>
    {
        public SalvarImportacaoArquivoAlfabetizacaoCommandValidator()
        {
            RuleFor(x => x.ArquivoAlfabetizacao.AnoLetivo)
                .GreaterThan(0).WithMessage("Ano Letivo inválido");

            RuleFor(x => x.ArquivoAlfabetizacao.CodigoEOLEscola)
                .NotEmpty().WithMessage("Código EOL da UE inválido")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Código EOL da UE inválido");

            RuleFor(x => x.ArquivoAlfabetizacao.TaxaAlfabetizacao)
                .GreaterThan(0).WithMessage("Taxa de alfabetização inválida");
        }
    }
}
