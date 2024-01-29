using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunoPorFiltroQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTurmasAlunoPorFiltroQuery(string codigoAluno, int? anoLetivo, bool? filtrarSituacaoMatricula, bool tipoTurma = false)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            FiltrarSituacaoMatricula = filtrarSituacaoMatricula;
            TipoTurma = tipoTurma;
        }

        public string CodigoAluno { get; }
        public int? AnoLetivo { get; }
        public bool? FiltrarSituacaoMatricula { get; }
        public bool TipoTurma { get; }
    }

    public class ObterTurmasAlunoPorFiltroQueryValidator : AbstractValidator<ObterTurmasAlunoPorFiltroQuery>
    {
        public ObterTurmasAlunoPorFiltroQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado.");
        }        
    }
}
