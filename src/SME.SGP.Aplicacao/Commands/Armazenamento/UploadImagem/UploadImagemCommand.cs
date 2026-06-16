using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class UploadImagemCommand : IRequest<ArquivoArmazenadoDto>
    {
        public UploadImagemCommand(byte[] imagemBytes, TipoArquivo tipoArquivo, string nomeArquivo, string formato)
        {
            ImagemBytes = imagemBytes;
            TipoArquivo = tipoArquivo;
            NomeArquivo = nomeArquivo;
            Formato = formato;
        }

        public byte[] ImagemBytes { get; }
        public TipoArquivo TipoArquivo { get; }
        public string NomeArquivo { get; }
        public string Formato { get; }
    }

    public class UploadImagemCommandValidator : AbstractValidator<UploadImagemCommand>
    {
        public UploadImagemCommandValidator()
        {
            RuleFor(a => a.ImagemBytes)
                  .NotEmpty()
                  .WithMessage("A imagem deve ser informada para realizar o upload para o servidor")
                  .Must(bytes => bytes != null && bytes.Length > 0)
                  .WithMessage("A imagem não pode estar vazia");

            RuleFor(a => a.NomeArquivo)
                .NotEmpty()
                .WithMessage("O nome do arquivo deve ser informado para realizar o upload da imagem para o servidor");

            RuleFor(a => a.Formato)
                .NotEmpty()
                .WithMessage("O formato do arquivo deve ser informado para realizar o upload da imagem para o servidor");
        }
    }
}
