using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using FluentValidation;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaCodigoQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterComponentesCurricularesPorTurmaCodigoQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }
    
    public class ObterComponentesCurricularesPorTurmaCodigoQueryValidator : AbstractValidator<ObterComponentesCurricularesPorTurmaCodigoQuery>
    {
        public ObterComponentesCurricularesPorTurmaCodigoQueryValidator()
        {

            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para obter componentes curriculares por turma.");
        }
    }
}
