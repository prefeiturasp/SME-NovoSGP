﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFechamentosPorTurmasCodigosBimestreQuery(string[] turmasCodigos, string alunoCodigo, int bimestre, DateTime? dataMatricula = null, DateTime? dataSituacao = null, int? anoLetivo = null)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            AnoLetivo = anoLetivo;
        }
        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public DateTime? DataMatricula { get; set; }
        public DateTime? DataSituacao { get; set; }
        public int? AnoLetivo { get; set; }

        public class ObterNotasFechamentosPorTurmasCodigosBimestreQueryValidator : AbstractValidator<ObterNotasFechamentosPorTurmasCodigosBimestreQuery>
        {
            public ObterNotasFechamentosPorTurmasCodigosBimestreQueryValidator()
            {
                RuleFor(a => a.TurmasCodigos)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Necessário informar os códigos de turmas para obter as notas de fechmamento");
                RuleFor(a => a.AlunoCodigo)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Necessário informar o código do aluno para obter as notas de fechmamento");
                RuleFor(a => a.Bimestre)
                    .NotNull()
                    .WithMessage("Necessário informar o bimestre para obter as notas de fechamento");
            }
        }
    }
}
