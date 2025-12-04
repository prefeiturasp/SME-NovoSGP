using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSituacaoAtendimentoNAAPAQuery : IRequest<SituacaoDto>
    {
        public ObterSituacaoAtendimentoNAAPAQuery(long id)
        {
            this.Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterSituacaoAtendimentoNAAPAQueryValidator : AbstractValidator<ObterSituacaoAtendimentoNAAPAQuery>
    {
        public ObterSituacaoAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do atendimento NAAPA deve ser informada para obter a situação do atendimento NAAPA.");
        }
    }
}
