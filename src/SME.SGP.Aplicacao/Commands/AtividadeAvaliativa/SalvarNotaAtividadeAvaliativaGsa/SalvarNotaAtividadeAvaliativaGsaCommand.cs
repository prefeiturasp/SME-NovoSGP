using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotaAtividadeAvaliativaGsaCommand : IRequest
    {
        public double? Nota { get; }
        public NotaConceito NotaConceito { get; set; }
        public long AtividadeId { get; set; }
        public StatusGSA StatusGsa { get; set; }
        public NotaTipoValor TipoNota { get; set; }
        public string CodigoAluno { get; set; }
        public string ComponenteCurricular { get; set; }

        public SalvarNotaAtividadeAvaliativaGsaCommand(NotaConceito notaConceito, double? nota, StatusGSA statusGsa, long atividadeId, NotaTipoValor tipoNota, string codigoAluno, string componenteCurricular)
        {
            Nota = nota;
            NotaConceito = notaConceito;
            StatusGsa = statusGsa;
            AtividadeId = atividadeId;
            TipoNota = tipoNota;
            CodigoAluno = codigoAluno;
            ComponenteCurricular = componenteCurricular;
        }
    }

    public class SalvarNotaAtividadeAvaliativaGsaCommandValidator : AbstractValidator<SalvarNotaAtividadeAvaliativaGsaCommand>
    {
    }
}