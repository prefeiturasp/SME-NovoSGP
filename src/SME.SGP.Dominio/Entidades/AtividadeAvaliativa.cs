using System;

namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativa : EntidadeBase
    {
        public int CategoriaId { get; set; }
        public int ComponenteCurricularId { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string DescricaoAvaliacao { get; set; }
        public string DreId { get; set; }
        public bool Excluido { get; set; }
        public int IipoAvaliacaoId { get; set; }
        public string NomeAvaliacao { get; set; }
        public string ProfessorRf { get; set; }
        public int TipoAvaliacaoId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Estra avaliação já está excluida.");
            Excluido = true;
        }
    }
}