using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQuery : IRequest<IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        public ObterAlunosAtivosTurmaProgramaPapEolQuery(int anoLetivo, string[] alunosCodigos)
        {
            AnoLetivo = anoLetivo;
            AlunosCodigos = alunosCodigos;
        }

        public int AnoLetivo { get; set; }
        public string[] AlunosCodigos { get; set; }
    }
    
    public class ObterAlunosAtivosTurmaProgramaPapEolQueryValidator : AbstractValidator<ObterAlunosAtivosTurmaProgramaPapEolQuery>
    {
        public ObterAlunosAtivosTurmaProgramaPapEolQueryValidator()
        {

            RuleFor(c => c.AlunosCodigos)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para realizar a consulta no EOL.");
        }
    }
}