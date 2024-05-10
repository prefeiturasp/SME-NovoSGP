using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoObservacaoPorObservacaoIdQuery : IRequest<DiarioBordoObservacaoDto>
    {
        public long ObservacaoId { get; set; }

        public ObterDiarioBordoObservacaoPorObservacaoIdQuery(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }
    }

    public class ObterDiarioBordoObservacaoPorObservacaoIdQueryValidator : AbstractValidator<ObterDiarioBordoObservacaoPorObservacaoIdQuery>
    {
        public ObterDiarioBordoObservacaoPorObservacaoIdQueryValidator()
        {
            RuleFor(x => x.ObservacaoId)
                .NotEmpty()
                .WithMessage("A observação deve ser informada para a busca do diário de bordo observação.");
        }
    }
}