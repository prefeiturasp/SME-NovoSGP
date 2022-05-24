using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroObterSupervisoresDto
    {
        [MinLength(2, ErrorMessage = "Nome deve conter no mínimo 2 caracteres")]
        public string Nome { get; set; }

        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}