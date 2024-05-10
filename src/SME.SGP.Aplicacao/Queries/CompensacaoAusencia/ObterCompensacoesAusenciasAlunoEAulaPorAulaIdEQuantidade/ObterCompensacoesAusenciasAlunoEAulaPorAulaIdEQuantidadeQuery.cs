using FluentValidation;
using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoAulaDto>>
    {
        public ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQuery(long aulaId, int quantidade)
        {
            AulaId = aulaId;
            Quantidade = quantidade;
        }

        public long AulaId { get; set; }
        public int Quantidade { get; set; }
    }

    public class ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidadeQueryValidator : AbstractValidator<ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQuery>
    {
        public ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidadeQueryValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("O identificador da aula deve ser informada para a alteração da compensação de ausência aluno e aula.");

            RuleFor(c => c.Quantidade)
                .NotEmpty()
                .WithMessage("A quantidade de aulas deve ser informada para a alteração da compensação de ausência aluno e aula.");
        }
    }
}