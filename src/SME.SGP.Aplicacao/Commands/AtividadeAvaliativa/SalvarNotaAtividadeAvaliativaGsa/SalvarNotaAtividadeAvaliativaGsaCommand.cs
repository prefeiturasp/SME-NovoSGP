using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotaAtividadeAvaliativaGsaCommand : IRequest
    {
        public double Nota { get; }
        public long NotaConceitoId { get; set; }
        public StatusGSA StatusGsa { get; set; }

        public SalvarNotaAtividadeAvaliativaGsaCommand(long notaConceitoId, double nota, StatusGSA statusGsa)
        {
            Nota = nota;
            NotaConceitoId = notaConceitoId;
            StatusGsa = statusGsa;
        }
    }

    public class
        SalvarNotaAtividadeAvaliativaGsaCommandValidator : AbstractValidator<SalvarNotaAtividadeAvaliativaGsaCommand>
    {
    }
}