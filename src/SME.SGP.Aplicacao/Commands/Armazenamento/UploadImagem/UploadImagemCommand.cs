using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class UploadImagemCommand : IRequest<ArquivoArmazenadoDto>
    {
        public UploadImagemCommand(Image imagem, TipoArquivo tipoArquivo, string nomeArquivo, string formato)
        {
            Imagem = imagem;
            TipoArquivo = tipoArquivo;
            NomeArquivo = nomeArquivo;
            Formato = formato;
        }

        public Image Imagem { get; }
        public TipoArquivo TipoArquivo { get; }
        public string NomeArquivo { get; }
        public string Formato { get; }
    }

    public class UploadImagemCommandValidator : AbstractValidator<UploadImagemCommand>
    {
        public UploadImagemCommandValidator()
        {
            RuleFor(a => a.Imagem)
                .NotEmpty()
                .WithMessage("A imagem deve ser informada para realizar o upload para o servidor");

            RuleFor(a => a.NomeArquivo)
                .NotEmpty()
                .WithMessage("O nome do arquivo deve ser informado para realizar o upload da imagem para o servidor");

            RuleFor(a => a.Formato)
                .NotEmpty()
                .WithMessage("O formato do arquivo deve ser informado para realizar o upload da imagem para o servidor");
        }
    }
}
