using FluentValidation;
using MediatR;

namespace SME.SGP.OtimizarArquivos.Worker.Commands.ComprimirPdf
{
    public class ComprimirPdfCommand : IRequest<bool>
    {
        public string NomeArquivo { get; set; }
        public ComprimirPdfCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }
    }
    public class ComprimirPdfCommandValidator : AbstractValidator<ComprimirPdfCommand>
    {
        public ComprimirPdfCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para otimização.");
        }

    }
}
