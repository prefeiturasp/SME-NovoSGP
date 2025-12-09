using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoAtendimentoNaapaPorIdQuery : IRequest<EncaminhamentoNAAPA>
    {
        public AlterarSituacaoAtendimentoNaapaPorIdQuery(long id)
        {
            Id = id;
        }
        

        public long Id { get; }
    }

    public class AlterarSituacaoAtendimentoNaapaPorIdQueryValidator : AbstractValidator<AlterarSituacaoAtendimentoNaapaPorIdQuery>
    {
        public AlterarSituacaoAtendimentoNaapaPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do atendimento NAAPA deve ser informado para a pesquisa.");
        }
    }
}
