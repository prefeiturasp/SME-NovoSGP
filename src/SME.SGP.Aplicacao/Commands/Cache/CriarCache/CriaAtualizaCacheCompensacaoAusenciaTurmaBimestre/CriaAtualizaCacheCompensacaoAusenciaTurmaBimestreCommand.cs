using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand : IRequest<bool>
    {
        public CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand(string codigoTurma, int bimestre, IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> totaisCompensacoesAusenciasAlunos)
        {
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
            TotaisCompensacoesAusenciasAlunos = totaisCompensacoesAusenciasAlunos;
        }

        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
        public IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto> TotaisCompensacoesAusenciasAlunos { get; set; }
    }

    public class CriaAtualizaCacheCompensacaoAusenciaTurmaCommandValidator : AbstractValidator<CriaAtualizaCacheCompensacaoAusenciaTurmaBimestreCommand>
    {

        public CriaAtualizaCacheCompensacaoAusenciaTurmaCommandValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma.");

            RuleFor(x => x.Bimestre)
                .NotEmpty()
                .WithMessage("É necessário informar o código da turma.");

            RuleFor(x => x.TotaisCompensacoesAusenciasAlunos)
                .NotEmpty()
                .WithMessage("Os totais de compensações devem ser informados.");
        }
    }
}
