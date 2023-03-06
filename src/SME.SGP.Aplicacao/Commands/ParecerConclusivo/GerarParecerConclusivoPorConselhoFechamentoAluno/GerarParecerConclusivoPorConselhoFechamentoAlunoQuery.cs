using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands
{
    public class GerarParecerConclusivoPorConselhoFechamentoAlunoCommand : IRequest<ParecerConclusivoDto>
    {
        public GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class GerarParecerConclusivoPorConselhoFechamentoAlunoCommandValidator : AbstractValidator<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>
    {
        public GerarParecerConclusivoPorConselhoFechamentoAlunoCommandValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotEmpty()
                .WithMessage("O código do fechamento deve ser informado para gerar seu parecer conclusivo");
            
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para gerar seu parecer conclusivo");
        }
    }
}