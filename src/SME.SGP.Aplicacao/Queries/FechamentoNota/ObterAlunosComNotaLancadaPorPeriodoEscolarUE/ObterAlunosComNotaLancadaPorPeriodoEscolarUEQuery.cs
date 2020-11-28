using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery : IRequest<IEnumerable<AlunosFechamentoNotaDto>>
    {
        public ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery(long ueId, long periodoEscolarId)
        {
            UeId = ueId;
            PeriodoEscolarId = periodoEscolarId;
        }
        public long UeId { get; internal set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ObterAlunosComNotaLancadaPorPeriodoEscolarUEQueryValidator : AbstractValidator<ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery>
    {
        public ObterAlunosComNotaLancadaPorPeriodoEscolarUEQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da UE");
            RuleFor(a => a.PeriodoEscolarId)
             .NotEmpty()
             .WithMessage("Necessário informar o Id do Periodo Escolar");
        }
    }
}
