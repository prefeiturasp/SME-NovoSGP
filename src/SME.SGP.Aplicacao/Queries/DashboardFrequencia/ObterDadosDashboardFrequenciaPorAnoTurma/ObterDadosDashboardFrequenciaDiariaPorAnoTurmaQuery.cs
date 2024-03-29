﻿using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery : IRequest<IEnumerable<FrequenciaAlunoDashboardDto>>
    {
        public ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery(FrequenciasConsolidadacaoPorTurmaEAnoDto frequenciaDto)
        {
            AnoLetivo = frequenciaDto.AnoLetivo;
            DreId = frequenciaDto.DreId;
            UeId = frequenciaDto.UeId;
            Modalidade = frequenciaDto.Modalidade;
            Semestre = frequenciaDto.Semestre;
            AnoTurma = frequenciaDto.AnoTurma;
            DataAula = frequenciaDto.DataAula;
            VisaoDre = frequenciaDto.VisaoDre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string AnoTurma { get; set; }
        public DateTime DataAula { get; set; }
        public bool VisaoDre { get; set; }
    }
    
    public class ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQueryValidator : AbstractValidator<ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery>
    {
        public ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a busca de frequência diária para dashboard.");

            RuleFor(x => x.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para a busca de frequência diária para dashboard.");

            RuleFor(x => x.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para a busca de frequência diária para dashboard.");
        }
    }
}
