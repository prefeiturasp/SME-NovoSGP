using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaRegistroAcaoCommand : IRequest<bool>
    {
        public ExcluirRespostaRegistroAcaoCommand(RespostaRegistroAcaoBuscaAtiva resposta)
        {
            Resposta = resposta;
        }

        public RespostaRegistroAcaoBuscaAtiva Resposta { get; }
    }

    public class ExcluirRespostaRegistroAcaoCommandValidator : AbstractValidator<ExcluirRespostaRegistroAcaoCommand>
    {
        public ExcluirRespostaRegistroAcaoCommandValidator()
        {
            RuleFor(c => c.Resposta)
            .NotEmpty()
            .WithMessage("A resposta do registro de ação deve ser informada para exclusão.");

        }
    }
}
