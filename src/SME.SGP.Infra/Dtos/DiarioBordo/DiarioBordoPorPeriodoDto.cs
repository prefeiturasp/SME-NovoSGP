using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DiarioBordoPorPeriodoDto
    {
        public long AulaId { get; set; }
        public long? DiarioBordoId { get; set; }
        public DateTime DataAula { get; set; }
        public bool Pendente { get; set; }
        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public string CodigoRf { get; set; }
        public string Nome { get; set; }
        public bool InseridoCJ { get; set; }
        public bool EhReposicao { get; set; }
        public int Tipo { get; set; }
        public string DescricaoComNome => string.IsNullOrEmpty(Nome) ? $"{DataAula:dd/MM/yyyy}" : $"{DataAula:dd/MM/yyyy} - {Nome} ({CodigoRf})";
        public string DescricaoCJ => InseridoCJ ? $"{DescricaoComNome} - CJ" : DescricaoComNome; 

        public string Titulo => Tipo == (int)TipoAula.Reposicao ? $"{DescricaoCJ} - Reposição" : DescricaoCJ;
    }
}
