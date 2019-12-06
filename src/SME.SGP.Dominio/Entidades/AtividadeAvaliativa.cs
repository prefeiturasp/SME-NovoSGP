using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativa : EntidadeBase
    {
        public List<AtividadeAvaliativaRegencia> AtividadeAvaliativaRegencia { get; set; }
        public int CategoriaId { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string DescricaoAvaliacao { get; set; }
        public int DisciplinaId { get; set; }
        public string DreId { get; set; }
        public bool EhRegencia { get; set; }
        public bool Excluido { get; set; }
        public string NomeAvaliacao { get; set; }
        public string ProfessorRf { get; set; }
        public TipoAvaliacao TipoAvaliacao { get; set; }
        public int TipoAvaliacaoId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }

        public void AdicionarAtividadeRegencia(AtividadeAvaliativaRegencia atividadeAvaliativaRegencia)
        {
            AtividadeAvaliativaRegencia.Add(atividadeAvaliativaRegencia);
        }

        public void AdicionarTipoAvaliacao(TipoAvaliacao tipoAvaliacao)
        {
            TipoAvaliacao = tipoAvaliacao;
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Estra avaliação já está excluida.");
            Excluido = true;
        }
    }
}