using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunoPorFiltroPlanoAeeQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTurmasAlunoPorFiltroPlanoAeeQuery(string codigoAluno, int? anoLetivo, bool? filtrarSituacaoMatricula, bool tipoTurma)
        {
            CodigoAluno = codigoAluno ?? throw new ArgumentNullException(nameof(codigoAluno));
            AnoLetivo = anoLetivo;
            FiltrarSituacaoMatricula = filtrarSituacaoMatricula;
            TipoTurma = tipoTurma;
        }

        public string CodigoAluno { get; }
        public int? AnoLetivo { get; }
        public bool? FiltrarSituacaoMatricula { get; }
        public bool TipoTurma { get; } 
    }
    public class ObterTurmasAlunoPorFiltroPlanoAeeQueryValidator : AbstractValidator<ObterTurmasAlunoPorFiltroQuery>
    {
        public ObterTurmasAlunoPorFiltroPlanoAeeQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
        }        
    }
}