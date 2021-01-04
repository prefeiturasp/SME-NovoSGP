using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaCommand : IRequest<RetornoBaseDto>
    {
        public long Id { get; set; }
    }

    public class ExcluirOcorrenciasCommandValidator : AbstractValidator<ExcluirOcorrenciaCommand>
    {
        public ExcluirOcorrenciasCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("A ocorrência deve ser informada para a exclusão.");
        }
    }
}