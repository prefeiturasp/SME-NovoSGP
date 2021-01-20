using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarNotificacaoDiarioBordoCommand : IRequest<AuditoriaDto>
    {
        public AlterarNotificacaoDiarioBordoCommand(long observacaoId, long usuarioId)
        {
            ObservacaoId = observacaoId;
            UsuarioId = usuarioId;
        }

        public long ObservacaoId { get; set; }
        public long UsuarioId { get; set; }
        
    }
}
