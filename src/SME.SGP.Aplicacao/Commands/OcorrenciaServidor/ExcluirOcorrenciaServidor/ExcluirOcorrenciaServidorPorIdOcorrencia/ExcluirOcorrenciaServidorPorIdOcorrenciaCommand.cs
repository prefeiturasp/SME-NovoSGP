using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirOcorrenciaServidorPorIdOcorrenciaCommand : IRequest<bool>
    {
        public ExcluirOcorrenciaServidorPorIdOcorrenciaCommand(long idOcorrencia)
        {
            IdOcorrencia = idOcorrencia;
        }

        public long IdOcorrencia { get; set; }
    }
    public class ExcluirOcorrenciaServidorPorIdOcorrenciaCommandValidator : AbstractValidator<ExcluirOcorrenciaServidorPorIdOcorrenciaCommand>
    {
        public ExcluirOcorrenciaServidorPorIdOcorrenciaCommandValidator()
        {
            RuleFor(x => x.IdOcorrencia)
                .NotEmpty()
                .WithMessage("A ocorrência deve ser informada para a exclusão.");
        }
    }
}