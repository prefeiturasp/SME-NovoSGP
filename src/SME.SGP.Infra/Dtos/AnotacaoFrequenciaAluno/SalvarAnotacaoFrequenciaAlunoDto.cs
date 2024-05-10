using FluentValidation;

namespace SME.SGP.Infra
{
    public class SalvarAnotacaoFrequenciaAlunoDto
    {
        public long? MotivoAusenciaId { get; set; }
        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public string Anotacao { get; set; }
        public string CodigoAluno { get; set; }
        public bool EhInfantil { get; set; }
    }


    public class SalvarAnotacaoFrequenciaAlunoDtoValidator : AbstractValidator<SalvarAnotacaoFrequenciaAlunoDto>
    {
        public SalvarAnotacaoFrequenciaAlunoDtoValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado.");

            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado.");

            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage(c => $"O código {(c.EhInfantil ? "da criança" : "do aluno")} deve ser informado.");

            RuleFor(c => c.MotivoAusenciaId)
                .NotEmpty()
                .WithMessage("A anotação ou o motivo da ausência devem ser informados.")
                .When(c => string.IsNullOrWhiteSpace(c.Anotacao));
        }
    }
}
