using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DiarioBordoResumoDto
    {
        public DiarioBordoResumoDto(long id, DateTime dataAula, string codigoRf, string nome, int tipo, long aulaId, bool inseridoCJ, bool pendente)
        {
            Id = id;
            CodigoRf = codigoRf;
            Nome = nome;
            DataAula = dataAula;
            Pendente = pendente;
            Tipo = tipo;
            AulaId = aulaId;
            InseridoCJ = inseridoCJ;
        }

        public long Id { get; set; }

        public DateTime DataAula { get; set; }

        public string CodigoRf { get; set; }

        public string Nome { get; set; }

        public bool Pendente { get; set; }
        public int Tipo { get; set; }
        public long AulaId { get; set; }
        public bool InseridoCJ { get; set; }

        public string DescricaoComNome => string.IsNullOrEmpty(Nome) ? $"{DataAula:dd/MM/yyyy}" : $"{DataAula:dd/MM/yyyy} - {Nome} ({CodigoRf})";

        public string DescricaoCJ => InseridoCJ ? $"{DescricaoComNome} - CJ" : DescricaoComNome;

        public string Descricao => Tipo == (int)TipoAula.Reposicao ? $"{DescricaoCJ} - Reposição" : DescricaoCJ;
    }
}
