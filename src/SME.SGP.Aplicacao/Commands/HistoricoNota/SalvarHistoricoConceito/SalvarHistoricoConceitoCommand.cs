using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoCommand : IRequest<long>
    {
        public SalvarHistoricoConceitoCommand(long conceitoAnteriorId, long conceitoNovoId, string criadoRF = "", string criadoPor = "")
        {
            ConceitoAnteriorId = conceitoAnteriorId;
            ConceitoNovoId = conceitoNovoId;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
        }

        public long ConceitoAnteriorId { get; set; }
        public long ConceitoNovoId { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
    }

    public class SalvarHIstoricoConceitoCommandValidator : AbstractValidator<SalvarHistoricoConceitoCommand>
    {
        public SalvarHIstoricoConceitoCommandValidator()
        {
            RuleFor(c => c.ConceitoAnteriorId)
            .NotEmpty()
            .WithMessage("O conceito anterior deve ser informado para geração do histórico.");

            RuleFor(c => c.ConceitoNovoId)
            .NotEmpty()
            .WithMessage("O conceito novo deve ser informado para geração do histórico.");
        }
    }
}
