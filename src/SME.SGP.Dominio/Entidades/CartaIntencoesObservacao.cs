using System;

namespace SME.SGP.Dominio
{
    public class CartaIntencoesObservacao : EntidadeBase
    {
        public CartaIntencoesObservacao(string observacao, long cartaIntencoesId, long usuarioId)
        {
            Observacao = observacao;
            CartaIntencoesId = cartaIntencoesId;
            UsuarioId = usuarioId;
        }
        protected CartaIntencoesObservacao()
        {
        }

        public string Observacao { get; set; }
        public long CartaIntencoesId { get; set; }
        public long UsuarioId { get; set; }
        public bool Excluido { get; set; }

        public void ValidarUsuarioAlteracao(long usuarioId)
        {
            if (usuarioId != UsuarioId)
            {
                throw new NegocioException("Você não pode alterar essa observação.");
            }
        }

        public void Remover()
        {
            Excluido = true;
        }
    }
}
