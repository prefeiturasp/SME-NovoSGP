using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class ComunicadoTurmaDto
    {
        public string CodigoTurma { get; set; }
        public long ComunicadoId { get; set; }
        public bool Excluido { get; set; }

        public static explicit operator ComunicadoTurmaDto(ComunicadoTurma turma)
         => turma == null ? null : new ComunicadoTurmaDto
         {
             CodigoTurma = turma.CodigoTurma,
             ComunicadoId = turma.ComunicadoId,
             Excluido = turma.Excluido
         };
    }
}
