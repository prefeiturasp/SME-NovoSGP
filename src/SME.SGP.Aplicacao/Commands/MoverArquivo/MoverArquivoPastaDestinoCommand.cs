using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivoPastaDestinoCommand : IRequest<bool>
    {
        public MoverArquivoPastaDestinoCommand(TipoArquivo tipoArquivo, string textoEditor)
        {
            TipoArquivo = tipoArquivo;
            TextoEditor = textoEditor;
        }

        public TipoArquivo TipoArquivo { get; set; }
        public string TextoEditor { get; set; }

        public class MoverArquivoPastaDestinoCommandValidator : AbstractValidator<MoverArquivoPastaDestinoCommand>
        {
            public MoverArquivoPastaDestinoCommandValidator()
            {
                RuleFor(a => a.TipoArquivo)
                    .NotEmpty()
                    .WithMessage("O registro Tipo Arquivo  deve ser informado");

                RuleFor(a => a.TextoEditor)
                    .NotEmpty()
                    .WithMessage("O registro Texto Editor  deve ser informado");
            }
        }
    }
}
