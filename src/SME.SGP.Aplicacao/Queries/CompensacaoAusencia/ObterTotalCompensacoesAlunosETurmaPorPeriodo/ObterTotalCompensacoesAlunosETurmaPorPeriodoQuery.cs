using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>>
    {
        public ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(int bimestre, List<string> alunos, string turmaCodigo, string professor = null)
        {
            Bimestre = bimestre;
            Alunos = alunos;
            TurmaCodigo = turmaCodigo;
            Professor = professor;
        }

        public int Bimestre { get; }
        public List<string> Alunos { get; }
        public string TurmaCodigo { get; }
        public string Professor { get; set; }
    }

    public class ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryValidator : AbstractValidator<ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery>
    {
        public ObterTotalCompensacoesAlunosETurmaPorPeriodoQueryValidator()
        {
            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de compensações no periodo");

            RuleFor(a => a.Alunos)
                .NotEmpty()
                .WithMessage("Os alunos devem ser informados para consulta de compensações no periodo");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O codigo da turma deve ser informado para consulta de compensações no periodo");
        }
    }
}
