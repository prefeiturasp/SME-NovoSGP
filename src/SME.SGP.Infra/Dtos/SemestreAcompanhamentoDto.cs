using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class SemestreAcompanhamentoDto
    {
        public SemestreAcompanhamentoDto(int semestre, string descricao, bool podeEditar)
        {
            Semestre = semestre;
            Descricao = descricao;
            PodeEditar = podeEditar;
        }

        public int Semestre { get; set; }
        public string Descricao { get; set; }
        public bool PodeEditar { get; set; }
    }
}
