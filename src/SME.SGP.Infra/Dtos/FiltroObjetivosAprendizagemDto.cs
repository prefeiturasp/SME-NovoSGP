using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroObjetivosAprendizagemDto
    {
        public FiltroObjetivosAprendizagemDto()
        {
            ComponentesCurricularesIds = new List<long>();
        }

        [Required(ErrorMessage = "O ano deve ser informado")]
        public string Ano { get; set; }

        public bool EnsinoEspecial { get; set; }

        [ListaTemElementos(ErrorMessage = "Os componentes curriculares devem ser informados")]
        public IList<long> ComponentesCurricularesIds { get; set; }
    }
}