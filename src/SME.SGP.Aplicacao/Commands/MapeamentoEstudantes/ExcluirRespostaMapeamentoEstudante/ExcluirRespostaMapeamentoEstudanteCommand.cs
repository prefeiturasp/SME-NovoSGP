using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaMapeamentoEstudanteCommand : IRequest<bool>
    {
        public ExcluirRespostaMapeamentoEstudanteCommand(RespostaMapeamentoEstudante resposta)
        {
            Resposta = resposta;
        }

        public RespostaMapeamentoEstudante Resposta { get; }
    }

    public class ExcluirRespostaMapeamentoEstudanteCommandValidator : AbstractValidator<ExcluirRespostaMapeamentoEstudanteCommand>
    {
        public ExcluirRespostaMapeamentoEstudanteCommandValidator()
        {
            RuleFor(c => c.Resposta)
            .NotEmpty()
            .WithMessage("A resposta do mapeamento de estudante deve ser informada para exclusão.");

        }
    }
}
