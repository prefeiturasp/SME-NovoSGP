using Dapper.Contrib.Extensions;
using System;

namespace SME.SGP.Dominio
{
    public class SecaoRelatorioSemestralPAP
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Obrigatorio { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
        public int Ordem { get; set; }
    }
}