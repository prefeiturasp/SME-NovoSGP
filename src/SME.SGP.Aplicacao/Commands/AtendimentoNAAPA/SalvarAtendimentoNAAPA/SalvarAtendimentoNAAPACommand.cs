using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtendimentoNAAPACommand : IRequest<bool>
    {
        public SalvarAtendimentoNAAPACommand(EncaminhamentoNAAPA encaminhamentoNAAPA)
        {
            EncaminhamentoNAAPA = encaminhamentoNAAPA;
        }

        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
    }

    public class SalvarAtendimentoNAAPACommandValidator : AbstractValidator<SalvarAtendimentoNAAPACommand>
    {
        public SalvarAtendimentoNAAPACommandValidator()
        {
            RuleFor(a => a.EncaminhamentoNAAPA)
               .NotEmpty()
               .WithMessage("O atendimento NAAPA deve ser informado para alteração.");
        }
    }
}
