using SME.SGP.Infra.Dtos;
using System;

namespace SME.SGP.Infra
{
    public class FiltroCodigoDataDto
    {
        public FiltroCodigoDataDto(string codigo, DateTime data)
        {
            Codigo = codigo;
            Data = data;
        }
        public string Codigo { get; set; }
        public DateTime Data { get; set; }
    }
}
