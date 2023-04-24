using System;

namespace SME.SGP.Infra
{
    public class RegistroFaltasNaoCompensadaDto
    {
        public string CodigoAluno { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public int NumeroAula { get; set; }
        public string Descricao { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
        public bool Sugestao { get; set; }
    }
}