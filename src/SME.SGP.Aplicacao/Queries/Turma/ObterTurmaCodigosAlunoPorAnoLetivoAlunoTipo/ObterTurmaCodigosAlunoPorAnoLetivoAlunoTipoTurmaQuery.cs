﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery : IRequest<string[]>
    {
        public ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(int anoLetivo, string codigoAluno, IEnumerable<int> tiposTurmas, bool? consideraHistorico = null, DateTime? dataReferencia = null, string ueCodigo = null, int? semestre = null)
        {
            AnoLetivo = anoLetivo;
            CodigoAluno = codigoAluno;
            TiposTurmas = tiposTurmas;
            DataReferencia = dataReferencia;
            ConsideraHistorico = VerificaConsideraHistorico(consideraHistorico);
            UeCodigo = ueCodigo;      
            Semestre = semestre;
        }
        public int AnoLetivo { get; set; }
        public string CodigoAluno { get; set; }
        public IEnumerable<int> TiposTurmas { get; set; }
        public DateTime? DataReferencia { get; }
        public bool ConsideraHistorico { get; set; }
        public string UeCodigo { get; set; }
        public int? Semestre { get; set; }

        private bool VerificaConsideraHistorico(bool? consideraHistorico)
        {
            if (consideraHistorico == null || !consideraHistorico.HasValue) return AnoLetivo < DateTime.Today.Year;
            else return consideraHistorico.Value;
        }
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
