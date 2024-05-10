using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoIDPorTurmaQuery : IRequest<long>
    {
        public ObterAcompanhamentoAlunoIDPorTurmaQuery(long turmaId, string alunoCodigo)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public string AlunoCodigo { get; }
    }

    public class ObterAcompanhamentoAlunoIDPorTurmaQueryValidator : AbstractValidator<ObterAcompanhamentoAlunoIDPorTurmaQuery>
    {
        public ObterAcompanhamentoAlunoIDPorTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para consulta do acompanhamento do aluno");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de seu acompanhamento");
        }
    }
}
