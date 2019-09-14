using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class WorkflowAprovacaoDto
    {
        public WorkflowAprovacaoDto()
        {
            Niveis = new List<WorkflowAprovacaoNivelDto>();
        }

        public int Ano { get; set; }
        public string DreId { get; set; }
        public List<WorkflowAprovacaoNivelDto> Niveis { get; set; }
        public NotificacaoCategoria NotificacaoCategoria { get; set; }

        [Required(ErrorMessage = "É necessário informar a mensagem da notificação.")]
        [MinLength(3, ErrorMessage = "Mensagem da notificação deve conter no mínimo 3 caracteres.")]
        public string NotificacaoMensagem { get; set; }

        public NotificacaoTipo NotificacaoTipo { get; set; }

        [Required(ErrorMessage = "É necessário informar o título da notificação.")]
        [MinLength(3, ErrorMessage = "O título da notificação deve conter no mínimo 3 caracteres.")]
        public string NotificacaoTitulo { get; set; }

        public string TurmaId { get; set; }
        public string UeId { get; set; }
    }
}