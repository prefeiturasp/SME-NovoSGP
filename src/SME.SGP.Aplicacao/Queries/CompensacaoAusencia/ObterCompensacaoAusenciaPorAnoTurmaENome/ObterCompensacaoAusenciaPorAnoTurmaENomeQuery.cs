using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaPorAnoTurmaENomeQuery : IRequest<CompensacaoAusencia>
    {
        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public string Atividade { get; set; }
        public string DisciplinaId { get; set; }
        public long Id { get; set; }

        public ObterCompensacaoAusenciaPorAnoTurmaENomeQuery(int anoLetivo, long turmaId, string atividade, long id, string disciplinaId)
        {
            AnoLetivo = anoLetivo;
            TurmaId = turmaId;
            Atividade = atividade;
            Id = id;
            DisciplinaId = disciplinaId;
        }
    }

    public class ObterCompensacaoAusenciaPorAnoTurmaENomeQueryValidator : AbstractValidator<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery>
    {
        public ObterCompensacaoAusenciaPorAnoTurmaENomeQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informado.");

            RuleFor(x => x.Atividade)
                .NotEmpty()
                .WithMessage("A atividade deve ser informado.");            
            RuleFor(x => x.DisciplinaId)
                .NotEmpty()
                .WithMessage("A disciplina deve ser informada.");
        }
    }
}