using System;

namespace SME.SGP.Infra
{
    public class JustificativaAlunoDto
    {
        public JustificativaAlunoDto(DateTime data, string motivo, long id)
        {
            Data = data;
            Motivo = motivo;
            Id = id;
        }

        public DateTime Data { get; set; }
        public string Motivo { get; set; }
        public long Id { get; set; }
    }
}
