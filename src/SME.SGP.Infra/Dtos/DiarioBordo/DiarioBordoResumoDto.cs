using System;

namespace SME.SGP.Infra
{
    public class DiarioBordoResumoDto
    {
        public DiarioBordoResumoDto(long id, DateTime dataAula, string codigoRf, string nome, bool pendente)
        {
            Id = id;
            CodigoRf = codigoRf;
            Nome = nome;
            DataAula = dataAula;
            Pendente = pendente;
        }

        public long Id { get; set; }

        public DateTime DataAula { get; set; }

        public string CodigoRf { get; set; }

        public string Nome { get; set; }

        public bool Pendente { get; set; }

        public string Descricao => string.IsNullOrEmpty(Nome) ? $"{DataAula:dd/MM/yyyy}" : $"{DataAula:dd/MM/yyyy} - {Nome} ({CodigoRf})";
    }
}
