using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class ComunicadoAlunoDto
    {
        public string AlunoCodigo { get; set; }
        public long ComunicadoId { get; set; }

        public static explicit operator ComunicadoAlunoDto(ComunicadoAluno aluno)
            => aluno == null ? null : new ComunicadoAlunoDto()
            {
                AlunoCodigo = aluno.AlunoCodigo,
                ComunicadoId = aluno.ComunicadoId
            };
    }
}
