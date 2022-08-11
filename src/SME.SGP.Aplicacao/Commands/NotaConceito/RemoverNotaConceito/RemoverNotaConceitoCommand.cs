using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RemoverNotaConceitoCommand : IRequest<bool>
    {
        public RemoverNotaConceitoCommand(NotaConceito notaConceito)
        {
            NotaConceito = notaConceito;
        }

        public NotaConceito NotaConceito { get; set; }
    }

    public class RemoverNotaConceitoCommandValidator : AbstractValidator<RemoverNotaConceitoCommand>
    {
        public RemoverNotaConceitoCommandValidator()
        {
            RuleFor(x => x.NotaConceito).NotEmpty().WithMessage("Informe uma Nota Conceito para Remover a Nota Conceito");
        }
    }
}