using System;

namespace SME.SGP.Infra
{
    public class JustificativaAlunoDto
    {
        public JustificativaAlunoDto()
        {

        }
        public JustificativaAlunoDto(long id, string motivo, DateTime dataAusencia, string registradoPor)
        {
            DataAusencia = dataAusencia;
            Motivo = motivo;
            Id = id;
            RegistradoPor = registradoPor;
        }

        public DateTime DataAusencia { get; set; }
        public string Motivo { get; set; }
        public string RegistradoPor { get; set; }
        public long Id { get; set; }
    }
}
