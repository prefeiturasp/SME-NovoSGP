﻿using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQuery : IRequest<ConsolidadoAtendimentoNAAPA>
    {
        public ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQuery(long ueId, int mes, int anoLetivo, string rfProfissional)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
            Mes = mes;
            RfProfissional = rfProfissional;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string RfProfissional { get; set; }
    }

    public class ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryValidator : AbstractValidator<ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQuery>
    {
        public ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo).GreaterThan(0).WithMessage("Informe o Ano Letivo para realizar a consulta");
            RuleFor(x => x.Mes).GreaterThan(0).WithMessage("Informe o Mes do Ano Letivo para realizar a consulta");
            RuleFor(x => x.UeId).GreaterThan(0).WithMessage("Informe o Id da Ue para realizar a consulta");
            RuleFor(x => x.RfProfissional).NotEmpty().WithMessage("Informe o Profissional para realizar a consulta");
        }
    }
}