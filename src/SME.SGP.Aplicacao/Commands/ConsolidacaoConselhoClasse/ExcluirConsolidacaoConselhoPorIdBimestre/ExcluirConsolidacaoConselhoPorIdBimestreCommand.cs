using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoConselhoPorIdBimestreCommand : IRequest<bool>
    {
        public long[] ConsolidacaoConselhoNotasIds { get; set; }
        public long[] ConsolidacaoConselhoAlunoTurmaIds { get; set; }

        public ExcluirConsolidacaoConselhoPorIdBimestreCommand(long[] consolidacaoConselhoNotasIds, long[] consolidacaoConselhoAlunoIds)
        {
            ConsolidacaoConselhoNotasIds = consolidacaoConselhoNotasIds;
            ConsolidacaoConselhoAlunoTurmaIds = consolidacaoConselhoAlunoIds;
        }
    }

    public class ExcluirConsolidacaoConselhoPorIdBimestreCommandValidator : AbstractValidator<ExcluirConsolidacaoConselhoPorIdBimestreCommand>
    {
        public ExcluirConsolidacaoConselhoPorIdBimestreCommandValidator()
        {
            RuleFor(a => a.ConsolidacaoConselhoNotasIds)
                .NotEmpty()
                .WithMessage("É necessário informar o(s) id(s) da consolidação feita erroneamente ao estudante para exclusão");
        }
    }
}
