using SME.SGP.Infra.Dtos;
using System;

namespace SME.SGP.Infra
{
    public class FiltroIdDataDto: FiltroIdDto
    {
        public FiltroIdDataDto(long id, DateTime data): base(id)
        {
            Data = data;
        }
        public DateTime Data { get; set; }
    }
}
