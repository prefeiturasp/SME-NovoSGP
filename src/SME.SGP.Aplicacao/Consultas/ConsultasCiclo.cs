using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCiclo : IConsultasCiclo
    {
        public IEnumerable<CicloDto> Listar(IEnumerable<int> idsTurmas)
        {
            //TODO obter lista de ciclos da API EOL
            return new List<CicloDto>
            {
                new CicloDto()
                {
                    Descricao="Alfabetização",
                    Id=1
                },
                new CicloDto()
                {
                    Descricao="Interdisciplinar",
                    Id=2
                }
            };
        }
    }
}