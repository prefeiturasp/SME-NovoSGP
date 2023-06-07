using FluentValidation;
using MediatR;
using System.IO;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirImagemCommand : IRequest<bool>
    {
        public ComprimirImagemCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }

        public string NomeArquivo { get; set; }
    }

    public class OtimizarImagensCommandValidator : AbstractValidator<ComprimirImagemCommand>
    {
        public OtimizarImagensCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para otimização.");
        }

    }
}
