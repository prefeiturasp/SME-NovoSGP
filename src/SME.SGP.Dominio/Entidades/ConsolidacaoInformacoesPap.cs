using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    [Table("consolidacao_informacoes_pap")]
    public class ConsolidacaoInformacoesPap
    {
        protected ConsolidacaoInformacoesPap() { }

        public ConsolidacaoInformacoesPap(
            int id,
            TipoPap tipoPap,
            string dreCodigo,
            string ueCodigo,
            string dreNome,
            string ueNome,
            int quantidadeTurmas,
            int quantidadeEstudantes,
            int quantidadeEstudantesComFrequenciaInferiorLimite,
            int dificuldadeAprendizagemTop1,
            int dificuldadeAprendizagemTop2,
            int outrasDificuldadesAprendizagem,
            string nomeDificuldadeTop1 = "",
            string nomeDificuldadeTop2 = "")
        {
            Id = id;
            TipoPap = tipoPap;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            DreNome = dreNome;
            UeNome = ueNome;
            QuantidadeTurmas = quantidadeTurmas;
            QuantidadeEstudantes = quantidadeEstudantes;
            QuantidadeEstudantesComFrequenciaInferiorLimite = quantidadeEstudantesComFrequenciaInferiorLimite;
            QuantidadeEstudantesDificuldadeTop1 = dificuldadeAprendizagemTop1;
            QuantidadeEstudantesDificuldadeTop2 = dificuldadeAprendizagemTop2;
            OutrasDificuldadesAprendizagem = outrasDificuldadesAprendizagem;
            NomeDificuldadeTop1 = nomeDificuldadeTop1;
            NomeDificuldadeTop2 = nomeDificuldadeTop2;
        }

        [Column("id")]
        public int Id { get; set; }

        [NotMapped]
        public TipoPap TipoPap { get; set; }

        [Column("tipo_pap")]
        public string TipoPapNome
        {
            get => TipoPap.ObterNome();
            set => TipoPap = Enum.GetValues(typeof(TipoPap))
                                 .Cast<TipoPap>()
                                 .FirstOrDefault(e => e.ObterNome() == value);
        }

        [Column("quantidade_turmas")]
        public int QuantidadeTurmas { get; set; }

        [Column("quantidade_estudantes")]
        public int QuantidadeEstudantes { get; set; }

        [Column("quantidade_estudantes_com_frequencia_inferior_limite")]
        public int QuantidadeEstudantesComFrequenciaInferiorLimite { get; set; }

        [Column("dre_codigo")]
        public string DreCodigo { get; set; }

        [Column("dre_nome")]
        public string DreNome { get; set; }

        [Column("ue_codigo")]
        public string UeCodigo { get; set; }

        [Column("ue_nome")]
        public string UeNome { get; set; }

        [Column("quantidade_estudantes_dificuldade_top_1")]
        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }

        [Column("quantidade_estudantes_dificuldade_top_2")]
        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }

        [Column("outras_dificuldades_aprendizagem")]
        public int OutrasDificuldadesAprendizagem { get; set; }

        [Column("nome_dificuldade_top_1")]
        public string NomeDificuldadeTop1 { get; set; }

        [Column("nome_dificuldade_top_2")]
        public string NomeDificuldadeTop2 { get; set; }
    }
}
