using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery : IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(Modalidade modalidadeTurma, int anoLetivo)
        {
            ModalidadeTurma = modalidadeTurma;
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
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
