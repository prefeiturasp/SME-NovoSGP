using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoEncaminhamentoAEEPorIdQuery : IRequest<SituacaoAEE>
    {
        public ObterSituacaoEncaminhamentoAEEPorIdQuery(long encaminhamentoAeeId)
        {
            EncaminhamentoAeeId = encaminhamentoAeeId;
        }

        public long EncaminhamentoAeeId { get; }
    }
    public class ObterSituacaoEncaminhamentoAEEPorIdQueryValidator : AbstractValidator<ObterSituacaoEncaminhamentoAEEPorIdQuery>
    {
        public ObterSituacaoEncaminhamentoAEEPorIdQueryValidator()
        {

            RuleFor(c => c.EncaminhamentoAeeId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento AEE deve ser informado para obter a situação");

        }
    }
}
