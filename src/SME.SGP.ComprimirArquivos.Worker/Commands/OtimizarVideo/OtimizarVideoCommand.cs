using FluentValidation;
using MediatR;
using System.IO;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarVideoCommand : IRequest<bool>
    {
        public OtimizarVideoCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }

        public string NomeArquivo { get; set; }
    }

    public class OtimizarVideoCommandValidator : AbstractValidator<OtimizarVideoCommand>
    {
        public OtimizarVideoCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para otimização.");
        }

    }
}
