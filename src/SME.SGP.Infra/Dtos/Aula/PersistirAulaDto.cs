using FluentValidation;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class PersistirAulaDto
    {
        public DateTime DataAula { get; set; }

        public long CodigoComponenteCurricular { get; set; }

        public string NomeComponenteCurricular { get; set; }

        public string DisciplinaCompartilhadaId { get; set; }

        public int Id { get; set; }

        public int Quantidade { get; set; }

        public RecorrenciaAula RecorrenciaAula { get; set; }

        public TipoAula TipoAula { get; set; }

        public long TipoCalendarioId { get; set; }

        public string CodigoTurma { get; set; }

        public string CodigoUe { get; set; }

        public bool AulaCJ { get; set; }
        public bool EhRegencia { get; set; }
    }

    public class PersistirAulaDtoValidator : AbstractValidator<PersistirAulaDto>
    {
        public PersistirAulaDtoValidator()
        {
            RuleFor(c => c.DataAula)
               .NotEmpty()
               .WithMessage("A data deve ser informada.");

            RuleFor(c => c.CodigoComponenteCurricular)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");

            RuleFor(c => c.NomeComponenteCurricular)
                .NotEmpty()
                .WithMessage("O nome do componente curricular deve ser informado.");

            RuleFor(c => c.Quantidade)
                .NotEmpty()
                .WithMessage("A quantidade de aulas deve ser informada.");

            RuleFor(c => c.RecorrenciaAula)
                .IsInEnum()
                .WithMessage("O tipo de recorrência da aula deve ser informado.")
                .Equal(RecorrenciaAula.AulaUnica)
                .When(c => c.TipoAula == TipoAula.Reposicao)
                .WithMessage("Aula de reposição não pode ser recorrente.");

            RuleFor(c => c.TipoAula)
                .IsInEnum()
                .WithMessage("O tipo de aula deve ser informado.");

            RuleFor(c => c.TipoCalendarioId)
               .NotEmpty()
               .WithMessage("O tipo de calendário deve ser informado.");

            RuleFor(c => c.CodigoTurma)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.CodigoUe)
              .NotEmpty()
              .WithMessage("A UE deve ser informada.");
        }
    }

}
