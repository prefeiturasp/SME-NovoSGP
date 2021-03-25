using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ArquivoArmazenadoDto
    {
        public ArquivoArmazenadoDto() { }

        public ArquivoArmazenadoDto(long id, Guid codigo)
        {
            Id = id;
            Codigo = codigo;
        }

        public long Id { get; set; }
        public Guid Codigo { get; set; }

        public static implicit operator Guid(ArquivoArmazenadoDto dto)
            => dto.Codigo;
    }
}
