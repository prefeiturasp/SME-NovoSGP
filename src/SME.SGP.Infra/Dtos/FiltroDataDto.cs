using System;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroDataDto
    {
        public FiltroDataDto(DateTime data) 
        {
            Data = data;
        }
        public DateTime Data { get; set; }
    }
}
