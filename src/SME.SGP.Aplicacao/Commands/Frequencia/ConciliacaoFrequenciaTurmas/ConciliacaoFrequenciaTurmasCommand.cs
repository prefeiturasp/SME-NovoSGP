﻿using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasCommand : IRequest<bool>
    {
        public ConciliacaoFrequenciaTurmasCommand(DateTime data, string turmaCodigo, string componenteCurricularId, bool bimestral, bool mensal)
        {
            Data = data;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
            Bimestral = bimestral;
            Mensal = mensal;
        }

        public DateTime Data { get; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public bool Bimestral { get; set; }
        public bool Mensal { get; set; }
    }

    public class ConciliacaoFrequenciaTurmasCommandValidator : AbstractValidator<ConciliacaoFrequenciaTurmasCommand>
    {
        public ConciliacaoFrequenciaTurmasCommandValidator()
        {
            RuleFor(a => a.Data)
                .NotEmpty()
                .WithMessage("A data de execução deve ser informada para calculo da conciliação de frequência das turmas no ano");
        }
    }
}
