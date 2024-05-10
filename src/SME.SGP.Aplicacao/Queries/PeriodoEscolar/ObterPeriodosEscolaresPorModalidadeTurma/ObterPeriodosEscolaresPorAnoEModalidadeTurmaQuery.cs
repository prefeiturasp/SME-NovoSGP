using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery : IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(Modalidade modalidadeTurma, int anoLetivo, int semestre)
        {
            ModalidadeTurma = modalidadeTurma;
            AnoLetivo = anoLetivo;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public int Semestre { get; }
        public Modalidade ModalidadeTurma { get; set; }
    }


    public class ObterPeriodosEscolaresPorModalidadeTurmaQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>
    {
        public ObterPeriodosEscolaresPorModalidadeTurmaQueryValidator()
        {
            RuleFor(c => c.ModalidadeTurma)
                .IsInEnum()
                .WithMessage("A modalidade deve ser informada.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
