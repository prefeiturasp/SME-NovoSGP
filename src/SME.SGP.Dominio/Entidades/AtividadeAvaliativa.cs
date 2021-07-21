using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class AtividadeAvaliativa : EntidadeBase
    {
        public AtividadeAvaliativa()
        {
            AtividadeAvaliativaRegencia = new List<AtividadeAvaliativaRegencia>();
            Disciplinas = new List<AtividadeAvaliativaDisciplina>();
        }
        public List<AtividadeAvaliativaRegencia> AtividadeAvaliativaRegencia { get; set; }
        public List<AtividadeAvaliativaDisciplina> Disciplinas { get; set; }
        public CategoriaAtividadeAvaliativa Categoria { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public string DescricaoAvaliacao { get; set; }
        public string DreId { get; set; }
        public bool EhCj { get; set; }
        public bool EhRegencia { get; set; }
        public bool Excluido { get; set; }
        public string NomeAvaliacao { get; set; }
        public string ProfessorRf { get; set; }
        public TipoAvaliacao TipoAvaliacao { get; set; }
        public long TipoAvaliacaoId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public long? AtividadeClassroomId { get; set; }

        public void AdicionarAtividadeRegencia(AtividadeAvaliativaRegencia atividadeAvaliativaRegencia)
        {
            AtividadeAvaliativaRegencia.Add(atividadeAvaliativaRegencia);
        }
        public void Adicionar(AtividadeAvaliativaDisciplina atividadeAvaliativaDisciplina)
        {
            if (atividadeAvaliativaDisciplina != null)
                Disciplinas.Add(atividadeAvaliativaDisciplina);
        }
        public void AdicionarTipoAvaliacao(TipoAvaliacao tipoAvaliacao)
        {
            TipoAvaliacao = tipoAvaliacao;
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Esta avaliação já está excluida.");
            Excluido = true;
        }

        public void PodeSerAlterada(Usuario usuario)
        {
            if (EhCj && (usuario.EhProfessor() || usuario.EhProfessorCj()) && (usuario.CodigoRf != this.CriadoRF))
                throw new NegocioException("Você não pode alterar esta Atividade Avaliativa.");
        }
    }
}