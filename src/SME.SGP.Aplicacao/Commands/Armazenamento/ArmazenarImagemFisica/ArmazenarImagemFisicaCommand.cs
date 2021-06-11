using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarImagemFisicaCommand : IRequest<bool>
    {
        public ArmazenarImagemFisicaCommand(Image imagem, string nomeFisico, string nomeArquivo, string caminho, string formato)
        {
            Imagem = imagem;
            NomeFisico = nomeFisico;
            NomeArquivo = nomeArquivo;
            Caminho = caminho;
            Formato = formato;
        }

        public Image Imagem { get; }
        public string NomeFisico { get; }
        public string NomeArquivo { get; }
        public string Caminho { get; }
        public string Formato { get; }
    }

    public class ArmazenarImagemFisicaCommandValidator : AbstractValidator<ArmazenarImagemFisicaCommand>
    {
        public ArmazenarImagemFisicaCommandValidator()
        {
            RuleFor(a => a.Imagem)
                .NotEmpty()
                .WithMessage("A imagem deve ser informada para armazenamento no servidor");

            RuleFor(a => a.NomeFisico)
                .NotEmpty()
                .WithMessage("O nome da imagem deve ser informado para armazenamento no servidor");

            RuleFor(a => a.Caminho)
                .NotEmpty()
                .WithMessage("O caminho da imagem deve ser informado para armazenamento no servidor");

            RuleFor(a => a.Formato)
                .NotEmpty()
                .WithMessage("O formato da imagem deve ser informado para armazenamento no servidor");
        }
    }
}
