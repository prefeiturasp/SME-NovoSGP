using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoConselhoClasseCommand : IRequest<long>
    {
        public SalvarHistoricoConceitoConselhoClasseCommand(long conselhoClasseNotaId, long conceitoAnteriorId, long conceitoNovoId)
        {
            ConselhoClasseNotaId = conselhoClasseNotaId;
            ConceitoAnteriorId = conceitoAnteriorId;
            ConceitoNovoId = conceitoNovoId;
        }

        public long ConselhoClasseNotaId { get; set; }
        public long ConceitoAnteriorId { get; set; }
        public long ConceitoNovoId { get; set; }
    }

    public class SalvarHistoricoConceitoConselhoClasseCommandValidator : AbstractValidator<SalvarHistoricoConceitoConselhoClasseCommand>
    {
        public SalvarHistoricoConceitoConselhoClasseCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseNotaId)
                  .NotEmpty()
                  .WithMessage("O id da nota do consecho de classe deve ser informada para geração do histórico!");

            RuleFor(a => a.ConceitoAnteriorId)
                  .NotEmpty()
                  .WithMessage("O id do conceito anterior do conselho de classe deve ser informada para geração do histórico!");

            RuleFor(a => a.ConceitoNovoId)
                  .NotEmpty()
                  .WithMessage("O id do conceito novo do conselho de classe deve ser informada para geração do histórico!");
        }
    }
}
