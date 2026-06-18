using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAulaCommand : IRequest<PlanoAulaDto>
    {
        public SalvarPlanoAulaCommand(PlanoAulaDto dto)
        {
            PlanoAula = dto;
        }
        public PlanoAulaDto PlanoAula { get; set; }
    }


    public class SalvarPlanoAulaCommandValidator : AbstractValidator<SalvarPlanoAulaCommand>
    {
        public SalvarPlanoAulaCommandValidator()
        {

            RuleFor(c => c.PlanoAula)
                .NotEmpty()
                .WithMessage("Plano de aula deve ser informado");
        }
    }
}
