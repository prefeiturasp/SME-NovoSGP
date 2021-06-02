using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class ObjetivoAprendizagem
    {
        private string anoTurma;

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
            {"thirteenth", 13},
            {"fourteenth", 14},
            {"fifteenth", 15},
            {"sixteenth", 16},
            {"seventeenth", 17},
            {"eighteenth", 18},
            {"nineteenth", 19},
            {"twentieth", 19},
        };

        public int Ano => Anos[AnoTurma];
        public string AnoTurma
        {
            get => anoTurma.All(v => char.IsDigit(v)) ? Anos.Single(a => a.Value.Equals(Convert.ToInt32(anoTurma))).Key : anoTurma;
            set
            {
                anoTurma = value.All(v => char.IsDigit(v)) ? Anos.Single(a => a.Value.Equals(Convert.ToInt32(value))).Key : value;
            }

        }
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