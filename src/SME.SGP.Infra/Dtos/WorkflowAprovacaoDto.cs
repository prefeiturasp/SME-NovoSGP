using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Infra
{
    public class WorkflowAprovacaoDto : IValidatableObject
    {
        public WorkflowAprovacaoDto()
        {
            Niveis = new List<WorkflowAprovacaoNivelDto>();
        }

        public int Ano { get; set; }
        public string DreId { get; set; }
        public long EntidadeParaAprovarId { get; set; }
        public List<WorkflowAprovacaoNivelDto> Niveis { get; set; }
        public NotificacaoCategoria NotificacaoCategoria { get; set; }

        [Required(ErrorMessage = "É necessário informar a mensagem da notificação.")]
        [MinLength(3, ErrorMessage = "Mensagem da notificação deve conter no mínimo 3 caracteres.")]
        public string NotificacaoMensagem { get; set; }

        public NotificacaoTipo NotificacaoTipo { get; set; }

        [Required(ErrorMessage = "É necessário informar o título da notificação.")]
        [MinLength(3, ErrorMessage = "O título da notificação deve conter no mínimo 3 caracteres.")]
        public string NotificacaoTitulo { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Informe o tipo de Workflow")]
        public WorkflowAprovacaoTipo Tipo { get; set; }

        public string TurmaId { get; set; }
        public string UeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Niveis == null || Niveis.Count == 0)
                yield return new ValidationResult("No mínimo 1 nível deve ser informado.");

            if (Niveis != null)
            {
                if (Niveis.Count(a => a.Cargo.HasValue) > 0)
                {
                    if (string.IsNullOrEmpty(UeId))
                        yield return new ValidationResult("Este workflow possui níveis com cargo e é necessário informar a Ue.");
                }
            }

            if (Tipo != WorkflowAprovacaoTipo.Basica)
            {
                yield return new ValidationResult("Para este tipo de workflow, é necessário informar um Id de entidade para aprovar.");
            }
        }

        public void AdicionarNivel(Cargo cargo)
        {
            Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = cargo,
                Nivel = Niveis.Count() + 1
            });
        }
    }
}