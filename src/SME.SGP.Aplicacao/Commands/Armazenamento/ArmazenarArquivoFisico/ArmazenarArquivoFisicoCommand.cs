using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommand : IRequest<bool>
    {
        public ArmazenarArquivoFisicoCommand(IFormFile arquivo, string nomeFisico, string caminho)
        {
            Arquivo = arquivo;
            NomeFisico = nomeFisico;
            Caminho = caminho;
        }

        public IFormFile Arquivo { get; set; }
        public string NomeFisico { get; set; }
        public string Caminho { get; set; }
    }

    public class ArmazenarArquivoFisicoCommandValidator : AbstractValidator<ArmazenarArquivoFisicoCommand>
    {
        public ArmazenarArquivoFisicoCommandValidator()
        {
            RuleFor(c => c.Arquivo)
            .NotEmpty()
            .WithMessage("O arquivo deve ser informado para armazenamento.");

            RuleFor(c => c.NomeFisico)
            .NotEmpty()
            .WithMessage("O nome deve ser informado para armazenamento.");

            RuleFor(c => c.Caminho)
            .NotEmpty()
            .WithMessage("O caminho deve ser informado para armazenamento.");
        }

    }
}
