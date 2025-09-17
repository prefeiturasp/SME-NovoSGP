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
            int quantidadeTurmas,
            int quantidadeEstudantes,
            int quantidadeEstudantesComMenosDe75PorcentoFrequencia,
            int dificuldadeAprendizagem1,
            int dificuldadeAprendizagem2,
            int outrasDificuldadesAprendizagem)
        {
            Id = id;
            TipoPap = tipoPap;
            QuantidadeTurmas = quantidadeTurmas;
            QuantidadeEstudantes = quantidadeEstudantes;
            QuantidadeEstudantesComMenosDe75PorcentoFrequencia = quantidadeEstudantesComMenosDe75PorcentoFrequencia;
            DificuldadeAprendizagem1 = dificuldadeAprendizagem1;
            DificuldadeAprendizagem2 = dificuldadeAprendizagem2;
            OutrasDificuldadesAprendizagem = outrasDificuldadesAprendizagem;
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

        [Column("quantidade_estudantes_com_menos_75_por_cento_frequencia")]
        public int QuantidadeEstudantesComMenosDe75PorcentoFrequencia { get; set; }

        [Column("dificuldade_aprendizagem_1")]
        public int DificuldadeAprendizagem1 { get; set; }

        [Column("dificuldade_aprendizagem_2")]
        public int DificuldadeAprendizagem2 { get; set; }

        [Column("outras_dificuldades_aprendizagem")]
        public int OutrasDificuldadesAprendizagem { get; set; }
    }
}
