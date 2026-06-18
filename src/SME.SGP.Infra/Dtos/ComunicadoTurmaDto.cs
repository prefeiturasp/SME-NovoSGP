using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Infra.Dtos
{
    public class ComunicadoTurmaDto
    {
        public string CodigoTurma { get; set; }
        public long ComunicadoId { get; set; }
        public bool Excluido { get; set; }

        public static explicit operator ComunicadoTurmaDto(ComunicadoTurma turma)
         => turma.EhNulo() ? null : new ComunicadoTurmaDto
         {
             CodigoTurma = turma.CodigoTurma,
             ComunicadoId = turma.ComunicadoId,
             Excluido = turma.Excluido
         };
    }
}
