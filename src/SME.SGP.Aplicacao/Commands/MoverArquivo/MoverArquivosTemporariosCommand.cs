using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class MoverArquivosTemporariosCommand : IRequest<string>
    {
        public MoverArquivosTemporariosCommand(TipoArquivo tipoArquivo, string textoEditorAtual,string textoEditorNovo)
        {
            TipoArquivo = tipoArquivo;
            TextoEditorAtual = textoEditorAtual;
            TextoEditorNovo = textoEditorNovo;
        }

        public TipoArquivo TipoArquivo { get; set; }
        public string TextoEditorAtual { get; set; }
        public string TextoEditorNovo { get; set; }

        public class MoverArquivoPastaDestinoCommandValidator : AbstractValidator<MoverArquivosTemporariosCommand>
        {
            public MoverArquivoPastaDestinoCommandValidator()
            {
                RuleFor(a => a.TipoArquivo)
                    .NotEmpty()
                    .WithMessage("O registro Tipo Arquivo  deve ser informado");

                RuleFor(a => a.TextoEditorNovo)
                    .NotEmpty()
                    .WithMessage("O registro Texto Editor  deve ser informado");

            }
        }
    }
}
