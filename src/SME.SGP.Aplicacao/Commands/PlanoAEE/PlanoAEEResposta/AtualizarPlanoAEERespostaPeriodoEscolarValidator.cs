using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{ 
    public class AtualizarPlanoAEERespostaPeriodoEscolarValidator : AbstractValidator<AtualizarPlanoAEERespostaPeriodoEscolarCommand>
    {
        public AtualizarPlanoAEERespostaPeriodoEscolarValidator()
        {
            RuleFor(x => x.RespostaId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Resposta deve ser informado!");
            RuleFor(x => x.RespostaPeriodoEscolar)
                   .NotEmpty()
                   .WithMessage("A Resposta do Plano deve ser informada!");
        }
    }
}
