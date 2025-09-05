using System.ComponentModel.DataAnnotations.Schema;

namespace SME.SGP.Dominio.Entidades
{
    [Table("consolidacao_alfabetizacao_nivel_escrita")]
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao : EntidadeBase
    {
        [Column("nivel_escrita")]
        public string NivelAlfabetizacao { get; set; }

        [NotMapped] // Não existe no banco, será calculado depois
        public string NivelAlfabetizacaoDescricao { get; set; }

        [Column("ue_codigo")]
        public string Ue { get; set; }

        [Column("dre_codigo")]
        public string Dre { get; set; }

        [Column("ano_letivo")]
        public int Ano { get; set; }

        [Column("quantidade")]
        public int TotalAlunos { get; set; }

        [Column("periodo")]
        public int Periodo { get; set; }
    }
}
