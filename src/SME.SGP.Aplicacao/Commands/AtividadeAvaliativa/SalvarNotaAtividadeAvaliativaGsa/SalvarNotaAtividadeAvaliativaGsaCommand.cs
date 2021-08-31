using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotaAtividadeAvaliativaGsaCommand : IRequest
    {
        public double Nota { get; }
        public long NotaConceitoId { get; set; }

        public SalvarNotaAtividadeAvaliativaGsaCommand(long notaConceitoId, double nota)
        {
            Nota = nota;
            NotaConceitoId = notaConceitoId;
        }
    }

    public class
        SalvarNotaAtividadeAvaliativaGsaCommandValidator : AbstractValidator<SalvarNotaAtividadeAvaliativaGsaCommand>
    {
    }
}