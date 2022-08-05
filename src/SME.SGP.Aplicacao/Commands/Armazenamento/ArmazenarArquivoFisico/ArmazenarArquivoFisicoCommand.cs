using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommand : IRequest<string>
    {
        public ArmazenarArquivoFisicoCommand(IFormFile arquivo, string nomeFisico, TipoArquivo tipoArquivo)
        {
            Arquivo = arquivo;
            NomeFisico = nomeFisico;
            TipoArquivo = tipoArquivo;
        }

        public IFormFile Arquivo { get; set; }
        public string NomeFisico { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
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

            RuleFor(c => c.TipoArquivo)
            .NotEmpty()
            .WithMessage("O caminho deve ser informado para armazenamento.");
        }

    }
}
