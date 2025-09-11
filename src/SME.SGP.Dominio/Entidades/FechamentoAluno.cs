using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class FechamentoAluno : EntidadeBase
    {
        public FechamentoAluno()
        {
            FechamentoNotas = new List<FechamentoNota>();
        }

        public long FechamentoTurmaDisciplinaId { get; set; }
        public FechamentoTurmaDisciplina FechamentoTurmaDisciplina { get; set; }   
        public string AlunoCodigo { get; set; }

        public AnotacaoFechamentoAluno AnotacaoFechamentoAluno { get; set; }

        public bool Excluido { get; set; }

        public List<FechamentoNota> FechamentoNotas { get; set; }

        public void AdicionarNota(FechamentoNota fechamentoNota)
        {
            if (fechamentoNota.NaoEhNulo())
                FechamentoNotas.Add(fechamentoNota);
        }
    }
}
