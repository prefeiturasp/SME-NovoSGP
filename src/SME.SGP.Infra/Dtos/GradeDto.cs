using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class GradeDto
    {
        public long Id { get; set; }

        [MaxLength(200, ErrorMessage = "O nome da grade deve conter no máximo 200 caracteres")]
        public string Nome { get; set; }
    }
}
