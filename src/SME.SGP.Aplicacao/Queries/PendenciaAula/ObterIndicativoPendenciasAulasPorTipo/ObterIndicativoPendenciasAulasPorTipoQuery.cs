using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQuery : IRequest<PendenciaPaginaInicialListao>
    {
        public ObterIndicativoPendenciasAulasPorTipoQuery(string disciplinaId, string turmaId, int bimestre)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public int Bimestre { get; }
    }

    public class ObterIndicativoPendenciasAulasPorTipoQueryValidator : AbstractValidator<ObterIndicativoPendenciasAulasPorTipoQuery>
    {
        public ObterIndicativoPendenciasAulasPorTipoQueryValidator()
        {
            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O código da disciplina deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.Bimestre)
            .NotEmpty()
            .WithMessage("O bimestre deve ser informado para consulta de pendência na aula.");

        }
    }
}
