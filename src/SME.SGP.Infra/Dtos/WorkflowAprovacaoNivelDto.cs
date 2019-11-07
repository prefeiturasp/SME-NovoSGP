using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Infra
{
    public class WorkflowAprovacaoNivelDto : IValidatableObject
    {
        public Cargo? Cargo { get; set; }

        [Required(ErrorMessage = "É necessário informar o nível.")]
        public int Nivel { get; set; }

        public string[] UsuariosRf { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UsuariosRf != null && (UsuariosRf.Count() > 0 && Cargo.HasValue))
                yield return new ValidationResult($"O nível {this.Nivel} deve conter apenas Cargo ou Usuários");

            if (UsuariosRf != null && (UsuariosRf.Count() == 0 && !Cargo.HasValue))
                yield return new ValidationResult($"O nível {this.Nivel} deve conter Cargo ou Usuários");
        }
    }
}