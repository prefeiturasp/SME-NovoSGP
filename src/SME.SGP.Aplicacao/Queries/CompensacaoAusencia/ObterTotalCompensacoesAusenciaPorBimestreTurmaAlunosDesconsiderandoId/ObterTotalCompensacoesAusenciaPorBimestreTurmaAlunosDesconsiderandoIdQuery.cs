using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>
    {
        public ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQuery(string codigoTurma, int bimestre, string[] codigosAlunos, long idCompensacaoDesconsiderado)
        {
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
            CodigosAlunos = codigosAlunos;
            IdCompensacaoDesconsiderado = idCompensacaoDesconsiderado;
        }

        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
        public string[] CodigosAlunos { get; set; }
        public long IdCompensacaoDesconsiderado { get; set; }
    }

    public class ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQueryValidator : AbstractValidator<ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQuery>
    {
        public ObterTotalCompensacoesAusenciaPorBimestreTurmaAlunosDesconsiderandoIdQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.Bimestre)
                .InclusiveBetween(1, 4)
                .WithMessage("O bimestre informado é inválido.");

            RuleFor(x => x.CodigosAlunos)
                .NotEmpty()
                .WithMessage("Informe os códigos dos alunos.");
        }
    }
}
