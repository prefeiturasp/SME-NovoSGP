using System;

namespace SME.SGP.Dominio
{
    public class CartaIntencoesObservacao : EntidadeBase
    {
        public CartaIntencoesObservacao(string observacao, long turmaId, long componenteCurricularId, long usuarioId )
        {
            Observacao = observacao;
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            UsuarioId = usuarioId;
        }
        protected CartaIntencoesObservacao()
        {
        }

        public string Observacao { get; set; }
        public long TurmaId { get; set; }
        public Turma Turma { get; set; }

        public long ComponenteCurricularId { get; set; }

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
