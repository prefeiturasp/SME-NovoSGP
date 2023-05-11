using FluentValidation;
using MediatR;
using System.IO;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class ComprimirVideoCommand : IRequest<bool>
    {
        public ComprimirVideoCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }

        public string NomeArquivo { get; set; }
    }

    public class OtimizarVideoCommandValidator : AbstractValidator<ComprimirVideoCommand>
    {
        public OtimizarVideoCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para otimização.");
        }

    }
}
