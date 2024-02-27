using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarParecerConclusivoCommand : IRequest<ParecerConclusivoDto>
    {
        public AlterarParecerConclusivoCommand(AlterarParecerConclusivoDto alterarParecerConclusivo)
        {
            ConselhoClasseId = alterarParecerConclusivo.ConselhoClasseId;
            FechamentoTurmaId = alterarParecerConclusivo.FechamentoTurmaId;
            AlunoCodigo = alterarParecerConclusivo.AlunoCodigo;
            ParecerConclusivoId = alterarParecerConclusivo.ParecerConclusivoId;
        }

        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public long? ParecerConclusivoId { get; set; }
    }

    public class AlterarParecerConclusivoCommandValidator : AbstractValidator<AlterarParecerConclusivoCommand>
    {
        public AlterarParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseId)
                .NotEmpty()
                .WithMessage("O id do conselho de classe deve ser informado para alteração do parecer conclusivo");

            RuleFor(a => a.FechamentoTurmaId)
                .NotEmpty()
                .WithMessage("O id do fechamento turma deve ser informado para alteração do parecer conclusivo");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para alteração do parecer conclusivo");
        }
    }
}
