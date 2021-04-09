using System;

namespace SME.SGP.Infra
{
    public class UeDetalhesParaSincronizacaoInstituicionalDto
    {
        public string UeCodigo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long DreCodigo { get; set; }
        public string UeNome { get; set; }
        public int TipoEscolaCodigo { get; set; }

    }
}
