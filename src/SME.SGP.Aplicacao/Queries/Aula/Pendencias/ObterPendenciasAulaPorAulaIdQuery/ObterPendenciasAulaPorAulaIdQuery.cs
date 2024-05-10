using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorAulaIdQuery : IRequest<long[]>
    {
        public ObterPendenciasAulaPorAulaIdQuery(long aulaId, Usuario usuarioLogado, bool temAtividadeAvaliativa = false, bool ehModalidadeInfantil = false)
        {
            AulaId = aulaId;
            TemAtividadeAvaliativa = temAtividadeAvaliativa;
            EhModalidadeInfantil = ehModalidadeInfantil;
            UsuarioLogado = usuarioLogado;
        }

        public long AulaId { get; set; }
        public bool EhModalidadeInfantil { get; set; }
        public bool TemAtividadeAvaliativa { get; set; }
        public Usuario UsuarioLogado { get; set; }
    }
}
