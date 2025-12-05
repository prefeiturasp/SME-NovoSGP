using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaAtendimentoNAAPACommand : IRequest<bool>
    {
        public ExcluirRespostaAtendimentoNAAPACommand(RespostaEncaminhamentoNAAPA resposta)
        {
            Resposta = resposta;
        }

        public RespostaEncaminhamentoNAAPA Resposta { get; }
    }

    public class ExcluirRespostaAtendimentoNAAPACommandValidator : AbstractValidator<ExcluirRespostaAtendimentoNAAPACommand>
    {
        public ExcluirRespostaAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.Resposta)
            .NotEmpty()
            .WithMessage("A resposta do atendimento naapa deve ser informada para exclusão.");

        }
    }
}
