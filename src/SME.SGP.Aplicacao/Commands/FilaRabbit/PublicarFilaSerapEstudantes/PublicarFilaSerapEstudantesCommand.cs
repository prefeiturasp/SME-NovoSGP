using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSerapEstudantesCommand : IRequest<bool>
    {
        public PublicarFilaSerapEstudantesCommand(string fila, object mensagem)
        {
            Fila = fila;
            Mensagem = mensagem;
        }

        public string Fila { get; set; }
        public object Mensagem { get; set; }
    }

    public class PublicarFilaSerapEstudantesCommandCommandValidator : AbstractValidator<PublicarFilaSerapEstudantesCommand>
    {
        public PublicarFilaSerapEstudantesCommandCommandValidator()
        {
            RuleFor(c => c.Fila)
               .NotEmpty()
               .WithMessage("O nome da fila deve ser informado para publicar na fila do Serap estudantes.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("O objeto da mensagem ser informado para publicar na fila do Serap estudantes.");

        }
    }
}
