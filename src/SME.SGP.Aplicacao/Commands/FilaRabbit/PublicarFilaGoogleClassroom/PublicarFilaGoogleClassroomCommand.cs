using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaGoogleClassroomCommand : IRequest<bool>
    {
        public PublicarFilaGoogleClassroomCommand(string fila, object mensagem)
        {
            Fila = fila;
            Mensagem = mensagem;
        }

        public string Fila { get; set; }
        public object Mensagem { get; set; }
    }

    public class PublicaFilaWorkerGoogleClassroomCommandValidator : AbstractValidator<PublicarFilaGoogleClassroomCommand>
    {
        public PublicaFilaWorkerGoogleClassroomCommandValidator()
        {
            RuleFor(c => c.Fila)
               .NotEmpty()
               .WithMessage("O nome da fila deve ser informado para publicar na fila da API do Google Classroom.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("O objeto da mensagem ser informado para publicar na fila da API do Google Classroom.");

        }
    }
}
