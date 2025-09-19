using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalInformacoesPapDto
    {
        public PainelEducacionalInformacoesPapDto(
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
        public TipoPap TipoPap { get; set; }

        [Column("tipo_pap")]
        public string TipoPapNome
        {
            get => TipoPap.ObterNome();
            set => TipoPap = Enum.GetValues(typeof(TipoPap))
                                 .Cast<TipoPap>()
                                 .FirstOrDefault(e => e.ObterNome() == value);
        }

        public int QuantidadeTurmas { get; set; }

        public int QuantidadeEstudantes { get; set; }

        public int QuantidadeEstudantesComFrequenciaInferiorLimite { get; set; }

        public string DreCodigo { get; set; }

        public string DreNome { get; set; }

        public string UeCodigo { get; set; }

        public string UeNome { get; set; }

        public int QuantidadeEstudantesDificuldadeTop1 { get; set; }

        public int QuantidadeEstudantesDificuldadeTop2 { get; set; }

        public int OutrasDificuldadesAprendizagem { get; set; }

        public string NomeDificuldadeTop1 { get; set; }

        public string NomeDificuldadeTop2 { get; set; }
    }
}
