using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAusenciaFechamentoCommand : IRequest<bool>
    {
        public SalvarPendenciaAusenciaFechamentoCommand(long turmaId, long componenteCurricularId, string professorRf, 
            string titulo, string mensagem, string instrucao, long? periodoEscolarId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            ProfessorRf = professorRf;
            Titulo = titulo;
            Mensagem = mensagem;
            Instrucao = instrucao;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public string ProfessorRf { get; set; }

        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Instrucao { get; set; }
    }

    public class SalvarPendenciaAusenciaFechamentoCommandValidator : AbstractValidator<SalvarPendenciaAusenciaFechamentoCommand>
    {
        public SalvarPendenciaAusenciaFechamentoCommandValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para geração da pendência de fechamento.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para geração da pendência de fechamento.");

            RuleFor(c => c.ProfessorRf)
               .NotEmpty()
               .WithMessage("O RF do professor deve ser informado para geração da pendência de fechamento.");

            RuleFor(c => c.Titulo)
               .NotEmpty()
               .WithMessage("O título deve ser informada para geração da pendência de fechamento.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("A mensagem deve ser informada para geração da pendência de fechamento.");

            RuleFor(c => c.Instrucao)
               .NotEmpty()
               .WithMessage("A instrução deve ser informada para geração da pendência de fechamento.");
        }
    }
}
