using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaDataDevolutivaPorTurmaComponenteQuery: IRequest<DateTime>
    {
        public ObterUltimaDataDevolutivaPorTurmaComponenteQuery(string turmaCodigo, long componenteCurricularCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
    }

    public class ObterUltimaDataDevolutivaPorTurmaComponenteQueryValidator : AbstractValidator<ObterUltimaDataDevolutivaPorTurmaComponenteQuery>
    {
        public ObterUltimaDataDevolutivaPorTurmaComponenteQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma para consulta de suas devolutivas");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .Must(a => a > 0)
                .WithMessage("É necessário informa o código do componente curriculas para consulta as devolutivas da turma");
        }
    }
}
