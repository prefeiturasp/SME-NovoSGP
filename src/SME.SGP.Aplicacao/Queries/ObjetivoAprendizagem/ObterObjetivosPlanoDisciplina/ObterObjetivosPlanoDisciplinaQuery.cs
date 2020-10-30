
using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPlanoDisciplinaQuery : IRequest<IEnumerable<ObjetivosAprendizagemPorComponenteDto>>
    {
        public ObterObjetivosPlanoDisciplinaQuery(int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool filtrarSomenteRegencia)
        {
            Bimestre = bimestre;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            DisciplinaId = disciplinaId;
            FiltrarSomenteRegencia = filtrarSomenteRegencia;
        }

        public DateTime DataReferencia { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long DisciplinaId { get; set; }
        public bool FiltrarSomenteRegencia { get; set; }
    }

    public class ObterObjetivosPlanoDisciplinaQueryValidator : AbstractValidator<ObterObjetivosPlanoDisciplinaQuery>
    {
        public ObterObjetivosPlanoDisciplinaQueryValidator()
        {
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre precisa ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma precisa ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular precisa ser informado.");       
        }
    }
}
