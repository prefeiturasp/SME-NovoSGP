using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class AdicionaFilaDto
    {
        public AdicionaFilaDto(string fila, object dados)
        {
            Fila = fila;
            Dados = dados;
        }

        public string Fila { get; set; }        
        public object Dados { get; set; }
    }
}
