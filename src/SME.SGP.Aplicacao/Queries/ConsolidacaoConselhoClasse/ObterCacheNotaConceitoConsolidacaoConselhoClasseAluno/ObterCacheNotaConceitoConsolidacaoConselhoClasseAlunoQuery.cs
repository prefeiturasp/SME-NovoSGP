using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery : IRequest<ConsolidadoConselhoClasseAlunoNotaCacheDto>
    {
        public ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery(long turmaId, long componenteCurricularId, int bimestre, string alunoCodigo)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            Bimestre = bimestre;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public long ComponenteCurricularId { get; }
        public int Bimestre { get; }
    }

    public class ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQueryValidator : AbstractValidator<ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQuery>
    {
        public ObterCacheNotaConceitoConsolidacaoConselhoClasseAlunoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
              .NotEmpty()
              .WithMessage("É necessário informar o id da turma do aluno para consultar o cache de notas para consolidações de conselho por turma e aluno");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para consultar o cache de notas para consolidações de conselho por turma e aluno");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consultar o cache de notas para consolidações de conselho por turma e aluno");

            RuleFor(a => a.Bimestre)
                .InclusiveBetween(0, 4)
                .WithMessage("É necessário informar um bimestre válido para consultar o cache de notas para consolidações de conselho por turma e aluno");
        }
    }
}
