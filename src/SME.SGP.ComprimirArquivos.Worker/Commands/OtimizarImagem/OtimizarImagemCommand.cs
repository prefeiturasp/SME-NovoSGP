using FluentValidation;
using MediatR;
using System.IO;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarImagemCommand : IRequest<bool>
    {
        public OtimizarImagemCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }

        public string NomeArquivo { get; set; }
    }

    public class OtimizarImagensCommandValidator : AbstractValidator<OtimizarImagemCommand>
    {
        public OtimizarImagensCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O nome do arquivo deve ser informado para otimização.");
        }

    }
}
