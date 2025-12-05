using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirConsolidadoAtendimentoNAAPAPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirConsolidadoAtendimentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirConsolidadoAtendimentoNAAPAPorIdCommand>
    {
        public ExcluirConsolidadoAtendimentoNAAPAPorIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Você deve informar um Id para exclusão da registro de consolidação");
        }
    }
}