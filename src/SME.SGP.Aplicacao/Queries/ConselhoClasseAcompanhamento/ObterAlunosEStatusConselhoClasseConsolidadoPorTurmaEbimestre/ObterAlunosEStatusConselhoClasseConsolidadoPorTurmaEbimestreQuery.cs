using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery : IRequest<List<ConselhoClasseAlunoDto>>
    {
        public ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery(long turmaId, int bimestre, int situacaoConselhoClasse)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            SituacaoConselhoClasse = situacaoConselhoClasse;
        }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public int SituacaoConselhoClasse { get; set; }
    }
    public class ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQueryValidator : AbstractValidator<ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQuery>
    {
        public ObterAlunosEStatusConselhoClasseConsolidadoPorTurmaEbimestreQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("O TurmaId deve ser informado.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado.");
            
            RuleFor(x => x.SituacaoConselhoClasse)
                .NotEmpty()
                .WithMessage("A Situação do conselho de classe deve ser informada.");

        }
    }
}
