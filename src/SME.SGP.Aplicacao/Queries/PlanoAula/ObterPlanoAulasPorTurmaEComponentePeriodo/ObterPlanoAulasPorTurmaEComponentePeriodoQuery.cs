using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao 
{
    public class ObterPlanoAulasPorTurmaEComponentePeriodoQuery : IRequest<IEnumerable<PlanoAulaRetornoDto>>
    {
        public ObterPlanoAulasPorTurmaEComponentePeriodoQuery(string turmaCodigo, string componenteCurricularCodigo, string componenteCurricularId, DateTime aulaInicio, DateTime aulaFim)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            ComponenteCurricularId = componenteCurricularId;
            AulaInicio = aulaInicio;
            AulaFim = aulaFim;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public DateTime AulaInicio { get; set; }
        public DateTime AulaFim { get; set; }
    }

    public class ObterPlanoAulasPorTurmaEComponentePeriodoQueryValidator : AbstractValidator<ObterPlanoAulasPorTurmaEComponentePeriodoQuery>
    {
        public ObterPlanoAulasPorTurmaEComponentePeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma para consulta de suas aulas!");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código do componente curricular para consulta de suas aulas!");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("Necessário informar o id do componente curricular para consulta de suas aulas!");

            RuleFor(a => a.AulaInicio)
                .NotEmpty()
                .WithMessage("Necessário informar a aula início para consulta de suas aulas!");

            RuleFor(a => a.AulaFim)
                .NotEmpty()
                .WithMessage("Necessário informar a aula fim para consulta de suas aulas!");
        }
    }
}
