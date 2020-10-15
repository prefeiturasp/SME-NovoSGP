using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPlanoDisciplinaQuery : IRequest<IEnumerable<ObjetivosAprendizagemPorComponenteDto>>
    {
        public ObterObjetivosPlanoDisciplinaQuery(DateTime dataReferencia, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool filtrarSomenteRegencia)
        {
            DataReferencia = dataReferencia;
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
            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência precisa ser informado.");
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre precisa ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma precisa ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular precisa ser informado.");
            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("O id da disciplina precisa ser informado.");
            RuleFor(a => a.FiltrarSomenteRegencia)
                .NotEmpty()
                .WithMessage("Precisa informar se é para filtrar somente regência.");
        }
    }
}
