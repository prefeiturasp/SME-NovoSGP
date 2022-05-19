using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery : IRequest<AnotacaoFechamentoAluno>
    {
        public ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            AlunoCodigo = alunoCodigo;
        }

        public long FechamentoTurmaDisciplinaId { get; }
        public string AlunoCodigo { get; }
    }

    public class ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQueryValidator : AbstractValidator<ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery>
    {
        public ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("O identificador do fechamento do aluno deve ser informado para consulta de anotação");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de anotação de seu fechamento");
        }
    }
}
