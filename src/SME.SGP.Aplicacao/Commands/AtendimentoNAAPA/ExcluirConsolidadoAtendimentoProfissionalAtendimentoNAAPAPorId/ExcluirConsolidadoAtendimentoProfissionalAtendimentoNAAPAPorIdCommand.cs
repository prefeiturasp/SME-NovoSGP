using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommand>
    {
        public ExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAPorIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Você deve informar um Id para exclusão da registro de consolidação");
        }
    }
}