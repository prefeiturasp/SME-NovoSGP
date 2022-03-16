using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand : IRequest<bool>
    {
        public SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand(long turmaId, long componenteCurricularId, long periodoEscolarId, string professorRf, string titulo, string mensagem, string instrucao, long ueId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
            ProfessorRf = professorRf;
            Titulo = titulo;
            Mensagem = mensagem;
            Instrucao = instrucao;
            UeId = ueId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public string ProfessorRf { get; set; }

        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Instrucao { get; set; }
        public long UeId { get; set; }
    }

    public class SalvarPendenciaAusenciaDeAvaliacaoProfessorCommandValidator : AbstractValidator<SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand>
    {
        public SalvarPendenciaAusenciaDeAvaliacaoProfessorCommandValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.PeriodoEscolarId)
               .Must(a => a > 0)
               .WithMessage("O periodo escolar deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.ProfessorRf)
               .NotEmpty()
               .WithMessage("O RF do professor deve ser informado para geração da pendência do professor.");

            RuleFor(c => c.Titulo)
               .NotEmpty()
               .WithMessage("O título deve ser informada para geração da pendência do professor.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("A mensagem deve ser informada para geração da pendência do professor.");

            RuleFor(c => c.Instrucao)
               .NotEmpty()
               .WithMessage("A instrução deve ser informada para geração da pendência do professor.");

            RuleFor(c => c.UeId)
               .NotEmpty()
               .WithMessage("A UE deve ser informada para geração da pendência do professor.");
        }
    }
}
