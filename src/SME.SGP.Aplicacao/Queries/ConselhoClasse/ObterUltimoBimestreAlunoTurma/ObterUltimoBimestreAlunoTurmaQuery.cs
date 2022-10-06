using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoBimestreAlunoTurmaQuery : IRequest<(int bimestre, bool possuiConselho, bool concluido)>
    {
        public Turma Turma { get; set; }
        public string AlunoCodigo { get; set; }

        public ObterUltimoBimestreAlunoTurmaQuery(Turma turma, string alunoCodigo = "")
        {
            Turma = turma;
            AlunoCodigo = alunoCodigo;
        }
    }

    public class ObterUltimoBimestreAlunoTurmaQueryValidator : AbstractValidator<ObterUltimoBimestreAlunoTurmaQuery>
    {
        public ObterUltimoBimestreAlunoTurmaQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("É necessário informar a turma para obter o último bimestre para verificação do conselho.");
        }
    }
}
