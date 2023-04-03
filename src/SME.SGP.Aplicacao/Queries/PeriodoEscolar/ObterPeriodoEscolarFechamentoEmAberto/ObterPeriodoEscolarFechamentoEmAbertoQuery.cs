using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarFechamentoEmAbertoQuery : IRequest<IEnumerable<PeriodoEscolar>>
    {
        public ObterPeriodoEscolarFechamentoEmAbertoQuery(string codigoTurma, ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataReferencia)
        {
            DataReferencia = dataReferencia;
            CodigoTurma = codigoTurma;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public string CodigoTurma { get; set; }
        public DateTime DataReferencia { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }

    public class ObterPeriodoEscolarFechamentoEmAbertoQueryValidator : AbstractValidator<ObterPeriodoEscolarFechamentoEmAbertoQuery>
    {
        public ObterPeriodoEscolarFechamentoEmAbertoQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta do período escolar com fechamento em aberto.");

            RuleFor(x => x.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada para consulta do período escolar com fechamento em aberto.");

            RuleFor(x => x.ModalidadeTipoCalendario)
                .NotEmpty()
                .WithMessage("A modalidade de tipo calendário da turma deve ser informado para consulta do período escolar com fechamento em aberto.");
        }
    }
}