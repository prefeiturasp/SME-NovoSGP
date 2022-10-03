﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunoPorFiltroQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterTurmasAlunoPorFiltroQuery(string codidoAluno, int? anoLetivo, bool? filtrarSituacaoMatricula, bool tipoTurma = false)
        {
            CodidoAluno = codidoAluno;
            AnoLetivo = anoLetivo;
            FiltrarSituacaoMatricula = filtrarSituacaoMatricula;
            TipoTurma = tipoTurma;
        }

        public string CodidoAluno { get; }
        public int? AnoLetivo { get; }
        public bool? FiltrarSituacaoMatricula { get; }
        public bool TipoTurma { get; }
    }

    public class ObterTurmasAlunoPorFiltroQueryValidator : AbstractValidator<ObterTurmasAlunoPorFiltroQuery>
    {
        public ObterTurmasAlunoPorFiltroQueryValidator()
        {
            RuleFor(c => c.CodidoAluno)
            .NotEmpty()
            .WithMessage("O código do aluno deve ser informado.");
        }        
    }
}
