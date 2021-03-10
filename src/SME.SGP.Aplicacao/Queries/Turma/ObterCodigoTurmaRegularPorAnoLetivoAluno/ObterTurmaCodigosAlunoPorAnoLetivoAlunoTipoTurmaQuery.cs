using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery : IRequest<IEnumerable<string>>
    {
        public ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(int anoLetivo, string codigoAluno, IEnumerable<int> tiposTurmas)
        {
            AnoLetivo = anoLetivo;
            CodigoAluno = codigoAluno;
            TiposTurmas = tiposTurmas;
        }
        public int AnoLetivo { get; set; }
        public string CodigoAluno { get; set; }
        public IEnumerable<int> TiposTurmas { get; set; }
    }

    public class ObterCodigoTurmaRegularPorAnoLetivoAlunoQueryValidator : AbstractValidator<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>
    {
        public ObterCodigoTurmaRegularPorAnoLetivoAlunoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("Necessário informar o ano letivo para obter o código da turma regular");
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno para obter o código da turma regular");
        }
    }
}
