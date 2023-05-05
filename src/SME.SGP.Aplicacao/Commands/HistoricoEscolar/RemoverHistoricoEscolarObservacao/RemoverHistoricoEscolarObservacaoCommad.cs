using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Commands.HistoricoEscolar
{
    public class RemoverHistoricoEscolarObservacaoCommad : IRequest<bool>
    {
        public RemoverHistoricoEscolarObservacaoCommad(HistoricoEscolarObservacao historicoObservacao)
        {
            HistoricoObservacao = historicoObservacao;
        }

        public HistoricoEscolarObservacao HistoricoObservacao { get; set; }
    }

    public class RemoverHistoricoEscolarObservacaoCommadValidator : AbstractValidator<RemoverHistoricoEscolarObservacaoCommad>
    {
        public RemoverHistoricoEscolarObservacaoCommadValidator()
        {
            RuleFor(f => f.HistoricoObservacao)
                .NotEmpty()
                .WithMessage("O histórico escolar observação deve ser informado para remoção.");
        }
    }
}
