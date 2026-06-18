using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class CartaIntencoesPersistenciaDto
    {
        public string CodigoTurma { get; set; }

        public long ComponenteCurricularId { get; set; }

        public List<CartaIntencoesDto> Cartas { get; set; }
    }
}
