using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class ObjetivoAprendizagem
    {
        private readonly Dictionary<string, int> Anos = new Dictionary<string, int>
        {           
            {"first", 1},
            {"second", 2},
            {"third", 3},
            {"fourth", 4},
            {"fifth", 5},
            {"sixth", 6},
            {"seventh", 7},
            {"eighth", 8},
            {"nineth", 9},
            {"tenth", 10},
            {"eleventh", 11},
            {"twelfth", 12},
            {"thirteenth", 13}
        };

        public int Ano => Anos[AnoTurma];
        public string AnoTurma { get; set; }
        public DateTime AtualizadoEm { get; set; }
        public string Codigo => CodigoCompleto?.Trim()?.Replace("(", "")?.Replace(")", "");
        public string CodigoCompleto { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public long Id { get; set; }

        public void Ativar()
        {
            Excluido = false;
        }

        public void Desativar()
        {
            Excluido = true;
        }
    }
}