using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAeeCommand : IRequest<ResultadoEncaminhamentoAEEDto>
    {
        public long TurmaId { get; set; }

        public RegistrarEncaminhamentoAeeCommand()
        {
        }
    }
    public class RegistrarEncaminhamentoAeeCommandValidator : AbstractValidator<RegistrarEncaminhamentoAeeCommand>
    {
        public RegistrarEncaminhamentoAeeCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada!");
        }
    }
}
