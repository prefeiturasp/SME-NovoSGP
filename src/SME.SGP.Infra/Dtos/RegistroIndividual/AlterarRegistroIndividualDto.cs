using FluentValidation;
using System;

namespace SME.SGP.Infra
{
    public class AlterarRegistroIndividualDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public string Registro { get; set; }
        public DateTime Data { get; set; }
    }

    public class AlterarRegistroIndividualDtoValidator : AbstractValidator<AlterarRegistroIndividualDto>
    {
        public AlterarRegistroIndividualDtoValidator()
        {
            RuleFor(a => a.TurmaId)
                  .NotEmpty()
                  .WithMessage("A turma deve ser informada!");

            RuleFor(a => a.ComponenteCurricularId)
                 .NotEmpty()
                 .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.AlunoCodigo)
                  .NotEmpty()
                  .WithMessage("O aluno deve ser informada!");

            RuleFor(a => a.Data)
                  .NotEmpty()
                  .WithMessage("A data deve ser informada!");

            RuleFor(a => a.Registro)
                   .NotEmpty()
                   .WithMessage("A descrição é obrigatória para o registro individual!");
        }
    }
}
