using System;

namespace SME.SGP.Infra
{
    public class FrequenciaDiariaAlunoDto
    {
        public FrequenciaDiariaAlunoDto(long? idAnotacao,DateTime dataAula, int quantidadeAulas, int quantidadePresenca, int quantidadeRemoto, int quantidadeAusencia, string justificaticaAusencia)
        {
            Id = idAnotacao;
            DataAula = dataAula;
            QuantidadeAulas = quantidadeAulas;
            QuantidadePresenca = quantidadePresenca;
            QuantidadeRemoto = quantidadeRemoto;
            QuantidadeAusencia = quantidadeAusencia;
            Motivo = justificaticaAusencia;
        }

        public long? Id { get; set; } // Id da Anotação
        public DateTime DataAula { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadePresenca { get; set; }
        public int QuantidadeRemoto { get; set; }
        public int QuantidadeAusencia { get; set; }
        public string Motivo { get; set; }
    }
}
