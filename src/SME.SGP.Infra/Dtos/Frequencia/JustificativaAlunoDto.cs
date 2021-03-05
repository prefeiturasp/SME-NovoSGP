using System;

namespace SME.SGP.Infra
{
    public class JustificativaAlunoDto
    {
        public JustificativaAlunoDto()
        {

        }
        public JustificativaAlunoDto(long id, string motivo, DateTime dataAnotacao, string registradoPor)
        {
            DataAnotacao = dataAnotacao;
            Motivo = motivo;
            Id = id;
            RegistradoPor = registradoPor;
        }

        public DateTime DataAnotacao { get; set; }
        public string Motivo { get; set; }
        public string RegistradoPor { get; set; }
        public long Id { get; set; }
    }
}
