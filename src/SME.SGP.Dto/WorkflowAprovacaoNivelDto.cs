using SME.SGP.Dominio;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class WorkflowAprovacaoNivelDto
    {
        public Cargo? Cargo { get; set; }

        [Required(ErrorMessage = "É necessário informar o nível.")]
        public int Nivel { get; set; }

        public string[] UsuariosRf { get; set; }
    }
}