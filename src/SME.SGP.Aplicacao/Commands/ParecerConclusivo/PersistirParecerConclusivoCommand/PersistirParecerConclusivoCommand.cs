using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class PersistirParecerConclusivoCommand : IRequest<bool>
    {
        public int? Bimestre { get; set; }
        public long TurmaId { get; set; }
        public long? ParecerConclusivoId { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }

        public PersistirParecerConclusivoCommand(PersistirParecerConclusivoDto persistirParecerConclusivoDto)
        {
            AlunoCodigo = persistirParecerConclusivoDto.ConselhoClasseAlunoCodigo;
            TurmaId = persistirParecerConclusivoDto.TurmaId;
            Bimestre = persistirParecerConclusivoDto.Bimestre;
            ParecerConclusivoId = persistirParecerConclusivoDto.ParecerConclusivoId;
            ConselhoClasseAlunoId = persistirParecerConclusivoDto.ConselhoClasseAlunoId;
            AnoLetivo = persistirParecerConclusivoDto.AnoLetivo;
            TurmaCodigo = persistirParecerConclusivoDto.TurmaCodigo;
        }
    }

    public class PersistirParecerConclusivoCommandValidator : AbstractValidator<PersistirParecerConclusivoCommand>
    {
        public PersistirParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("A turmaId deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.ParecerConclusivoId)
                .NotEmpty()
                .WithMessage("O parecer conclusivo deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("O código do conselho deve ser informado para gerar seu parecer conclusivo");

            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado para gerar seu parecer conclusivo");
        }
    }
}
