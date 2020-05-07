using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class SemestreAcompanhamentoDto
    {
        public SemestreAcompanhamentoDto(int semestre, string descricao)
        {
            Semestre = semestre;
            Descricao = descricao;
        }

        public int Semestre { get; set; }
        public string Descricao { get; set; }
    }
}
