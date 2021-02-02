using System;

namespace SME.SGP.Infra
{
    public class JustificativaAlunoDto
    {
        public JustificativaAlunoDto(long id, string motivo, DateTime dataAnotacao)
        {
            DataAnotacao = dataAnotacao;
            Motivo = motivo;
            Id = id;
        }

        public DateTime DataAnotacao { get; set; }
        public string Motivo { get; set; }
        public long Id { get; set; }
    }
}
