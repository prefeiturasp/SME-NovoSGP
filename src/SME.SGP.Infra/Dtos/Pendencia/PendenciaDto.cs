using Minio.DataModel;
using SME.SGP.Infra.Dtos;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class PendenciaPerfilUsuarioDto
    {
        public long Id { get; set; }
        public long PendenciaId { get; set; }
        public long UsuarioId { get; set; }
        public int PerfilCodigo { get; set; }
        public long? UeId { get; set; }
        public string CodigoRf { get; set; }
    }

    public class PendenciaPerfilUsuarioUePerfilDto
    {
        public int PerfilCodigo { get; set; }
        public long UeId { get; set; }
    }

    public static class PendenciaPerfilUsuarioExtension
    {
        public static bool FuncionarioPossuiPendenciaPerfil(this IEnumerable<PendenciaPerfilUsuarioDto> pendenciasPerfilUsuario, string rfFuncionario)
        => pendenciasPerfilUsuario.Any(p => p.CodigoRf.Equals(rfFuncionario));
    }
}
