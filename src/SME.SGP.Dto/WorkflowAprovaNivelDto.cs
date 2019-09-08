using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class WorkflowAprovaNivelDto
    {
        public WorkflowAprovaNivelDto()
        {
        }

        public int Ano { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição do nível.")]
        [MinLength(3, ErrorMessage = "Descrição deve conter no mínimo 3 caracteres.")]
        public string Descricao { get; set; }

        public string DreId { get; set; }
        public string EscolaId { get; set; }

        [Required(ErrorMessage = "É necessário informar o nível.")]
        public int Nivel { get; set; }

        public string TurmaId { get; set; }

        [Required(ErrorMessage = "É necessário informar o usuário.")]
        [MinLength(1, ErrorMessage = "É necessário informar o usuário.")]
        public string UsuarioId { get; set; }
    }
}