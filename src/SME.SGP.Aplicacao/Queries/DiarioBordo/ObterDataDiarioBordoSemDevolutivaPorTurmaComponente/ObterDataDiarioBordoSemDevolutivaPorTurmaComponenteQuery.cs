using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery : IRequest<DateTime?>
    {
        public ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery(string turmaCodigo, long componenteCurricularCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
    }

    public class ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQueryValidator : AbstractValidator<ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQuery>
    {
        public ObterDataDiarioBordoSemDevolutivaPorTurmaComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de diários de bordo.");

            RuleFor(c => c.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para consulta de diários de bordo.");
        }
    }
}
