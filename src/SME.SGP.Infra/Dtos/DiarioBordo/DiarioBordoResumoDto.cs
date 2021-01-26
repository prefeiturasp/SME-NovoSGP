using System;

namespace SME.SGP.Infra
{
    public class DiarioBordoResumoDto
    {
        public DiarioBordoResumoDto(long id, DateTime dataAula, string codigoRf, string nome)
        {
            Id = id;
            CodigoRf = codigoRf;
            Nome = nome;
            DataAula = dataAula;
        }

        public long Id { get; set; }

        public DateTime DataAula { get; set; }

        public string CodigoRf { get; set; }

        public string Nome { get; set; }

        public string Descricao => $"{DataAula:dd/MM/yyyy} - {Nome} ({CodigoRf})";
    }
}
