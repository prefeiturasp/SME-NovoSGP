using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasTurmaPorCodigoAlunoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterMatriculasTurmaPorCodigoAlunoQuery(string codigoAluno, DateTime? dataAula, int? anoLetivo)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            DataAula = dataAula;
        }

        public string CodigoAluno { get; }
        public int? AnoLetivo { get; }
        public DateTime? DataAula { get; }
    }

    public class ObterMatriculasTurmaPorCodigoAlunoQueryValidator : AbstractValidator<ObterMatriculasTurmaPorCodigoAlunoQuery>
    {
        public ObterMatriculasTurmaPorCodigoAlunoQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado para busca de matrículas/turma no EOL.");
        }        
    }
}
