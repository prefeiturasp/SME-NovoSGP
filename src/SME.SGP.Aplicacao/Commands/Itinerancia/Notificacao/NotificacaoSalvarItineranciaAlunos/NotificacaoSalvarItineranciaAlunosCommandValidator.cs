using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaAlunosCommandValidator : AbstractValidator<NotificacaoSalvarItineranciaAlunosCommand>
    {
        public NotificacaoSalvarItineranciaAlunosCommandValidator()
        {
            RuleFor(c => c.UeCodigo)
               .NotEmpty()
               .WithMessage("O Código da UE deve ser informado");
            RuleFor(c => c.CriadoRF)
               .NotEmpty()
               .WithMessage("O RF do Usuário deve ser informado");
            RuleFor(c => c.CriadoPor)
               .NotEmpty()
               .WithMessage("O Nome do Usuário deve ser informado");
            RuleFor(c => c.DataVisita)
               .NotEmpty()
               .WithMessage("A data da visita deve ser informada");
        }
    }
}
