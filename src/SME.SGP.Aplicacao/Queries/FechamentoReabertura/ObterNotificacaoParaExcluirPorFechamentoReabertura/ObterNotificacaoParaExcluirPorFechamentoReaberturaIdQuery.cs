using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQuery:IRequest<long>
    {
        public ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQuery(long fechamentoReaberturaId)
        {
            FechamentoReaberturaId = fechamentoReaberturaId;
        }

        public long FechamentoReaberturaId { get; set; }
    }

    public class ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQueryValidator : AbstractValidator<ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQuery>
    {
        public ObterNotificacaoParaExcluirPorFechamentoReaberturaIdQueryValidator()
        {
            RuleFor(x => x.FechamentoReaberturaId).NotEmpty().WithMessage("Informe o Id do Fechamanto para consultar se existe notificação");
        }
    }
}