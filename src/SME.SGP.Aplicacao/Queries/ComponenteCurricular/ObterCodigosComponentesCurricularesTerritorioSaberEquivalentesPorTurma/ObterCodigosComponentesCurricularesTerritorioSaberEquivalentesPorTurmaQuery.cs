using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery : IRequest<(string codigoComponente, string professor)[]>
    {
        public ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(long codigoComponenteBase, string codigoTurma, string professor,long turmaId = 0)
        {
            CodigoComponenteBase = codigoComponenteBase;
            CodigoTurma = codigoTurma;
            Professor = professor;
            TurmaId = turmaId;
        }

        public long CodigoComponenteBase { get; set; }
        public string CodigoTurma { get; set; }
        public long TurmaId { get; set; }
        public string Professor { get; set; }        
    }

    public class ObterComponenteCurricularTerritorioSaberEquivalentePorTurmaQueryValidator : AbstractValidator<ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery>
    {
        public ObterComponenteCurricularTerritorioSaberEquivalentePorTurmaQueryValidator()
        {
            RuleFor(x => x.CodigoComponenteBase)
                .GreaterThan(0)
                .WithMessage("É necessário informar o código do componente base para a busca.");

            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma.");
        }
    }
}
