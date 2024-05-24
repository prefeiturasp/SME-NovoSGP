using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtualizarPlanoAEERespostaPeriodoEscolarCommand : IRequest<bool>
    {
        public long RespostaId { get; set; }
        public string RespostaPeriodoEscolar { get; set; }

        public AtualizarPlanoAEERespostaPeriodoEscolarCommand(long respostaId, string respostaPeriodoEscolar)
        {
            RespostaId = respostaId;
            RespostaPeriodoEscolar = respostaPeriodoEscolar;
        }
    }
}
