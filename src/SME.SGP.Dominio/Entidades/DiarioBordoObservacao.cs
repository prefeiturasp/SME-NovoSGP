using System;

namespace SME.SGP.Dominio
{
    public class DiarioBordoObservacao : EntidadeBase
    {
        public DiarioBordoObservacao(string observacao, long diarioBordoId, long usuarioId)
        {
            Observacao = observacao;
            DiarioBordoId = diarioBordoId;
            UsuarioId = usuarioId;
        }
        protected DiarioBordoObservacao()
        {
        }

        public string Observacao { get; set; }
        public long DiarioBordoId { get; set; }
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
