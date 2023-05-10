using FluentValidation;
using MediatR;
using System.IO;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class MoverExcluirArquivoFisicoCommand : IRequest<bool>
    {
        public MoverExcluirArquivoFisicoCommand(string nomeArquivoOrigem, string nomeArquivoDestino)
        {
            NomeArquivoOrigem = nomeArquivoOrigem;
            NomeArquivoDestino = nomeArquivoDestino;
        } 

        public string NomeArquivoOrigem { get; set; }
        public string NomeArquivoDestino { get; set; }
    }

    public class MoverExcluirArquivoFisicoCommandValidator : AbstractValidator<MoverExcluirArquivoFisicoCommand>
    {
        public MoverExcluirArquivoFisicoCommandValidator()
        {
            RuleFor(c => c.NomeArquivoOrigem)
            .NotEmpty()
            .WithMessage("O nome do arquivo de origem deve ser informado para exclusão/remoção do arquivo.");
            
            RuleFor(c => c.NomeArquivoDestino)
                .NotEmpty()
                .WithMessage("O nome do arquivo de destino deve ser informado para exclusão/remoção do arquivo");
        }

    }
}
