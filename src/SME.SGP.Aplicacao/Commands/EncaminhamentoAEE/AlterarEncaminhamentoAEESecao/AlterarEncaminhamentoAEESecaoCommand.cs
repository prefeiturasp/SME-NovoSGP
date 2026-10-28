using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarEncaminhamentoAEESecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarEncaminhamentoAEESecaoCommand(EncaminhamentoAEESecao secao)
        {
            Secao = secao;
        }

        public EncaminhamentoAEESecao Secao { get; set; }
    }

    public class AlterarEncaminhamentoAEESecaoCommandValidator : AbstractValidator<AlterarEncaminhamentoAEESecaoCommand>
    {
        public AlterarEncaminhamentoAEESecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
                   .NotEmpty()
                   .WithMessage("A seção deve ser informada!");
        }
    }
}
