using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommand : IRequest<bool>
    {
        public ArmazenarImagemFisicaCommand(byte[] imagemBytes, string nomeFisico, string nomeArquivo, TipoArquivo tipoArquivo, string formato)
        {
            ImagemBytes = imagemBytes;
            NomeFisico = nomeFisico;
            NomeArquivo = nomeArquivo;
            TipoArquivo = tipoArquivo;
            Formato = formato;
        }

        public byte[] ImagemBytes { get; set; } = Array.Empty<byte>();
        public string NomeFisico { get; set; } = string.Empty;
        public string NomeArquivo { get; set; } = string.Empty;
        public TipoArquivo TipoArquivo { get; set; } 
        public string Formato { get; set; } = string.Empty;
    }

    public class ArmazenarImagemFisicaCommandValidator : AbstractValidator<ArmazenarImagemFisicaCommand>
    {
        public ArmazenarImagemFisicaCommandValidator()
        {
            RuleFor(a => a.ImagemBytes)
                .NotEmpty()
                .WithMessage("A imagem deve ser informada para armazenamento no servidor")
                .Must(bytes => bytes != null && bytes.Length > 0)
                .WithMessage("A imagem não pode estar vazia");

            RuleFor(a => a.NomeFisico)
                .NotEmpty()
                .WithMessage("O nome da imagem deve ser informado para armazenamento no servidor");

            RuleFor(a => a.TipoArquivo)
                .NotEmpty()
                .WithMessage("O caminho da imagem deve ser informado para armazenamento no servidor");

            RuleFor(a => a.Formato)
                .NotEmpty()
                .WithMessage("O formato da imagem deve ser informado para armazenamento no servidor");
        }
    }
}
