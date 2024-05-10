﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasAulasPorProfessorEComponenteQuery: IRequest<IEnumerable<DatasAulasDto>>
    {
        public ObterDatasAulasPorProfessorEComponenteQuery(string turmaCodigo, string componenteCurricularCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }

    public class ObterDatasAulasPorProfessorEComponenteQueryValidator: AbstractValidator<ObterDatasAulasPorProfessorEComponenteQuery>
    {
        public ObterDatasAulasPorProfessorEComponenteQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma para consulta de suas aulas!");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o código do componente curricular para consulta de suas aulas!");
        }
    }
}
