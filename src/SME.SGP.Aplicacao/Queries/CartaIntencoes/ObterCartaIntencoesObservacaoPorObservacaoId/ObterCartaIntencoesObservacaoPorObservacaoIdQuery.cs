using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesObservacaoPorObservacaoIdQuery : IRequest<CartaIntencoesObservacaoDto>
    {
        public long ObservacaoId { get; set; }

        public ObterCartaIntencoesObservacaoPorObservacaoIdQuery(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }
    }

    public class ObterCartaIntencoesObservacaoPorObservacaoIdQueryValidator : AbstractValidator<ObterCartaIntencoesObservacaoPorObservacaoIdQuery>
    {
        public ObterCartaIntencoesObservacaoPorObservacaoIdQueryValidator()
        {
            RuleFor(x => x.ObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada para a busca da observação da carta de intenções.");
        }
    }
}
