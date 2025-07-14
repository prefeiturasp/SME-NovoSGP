using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class Auditoria
    {
        public string Acao { get; set; }
        public long Chave { get; set; }
        public DateTime Data { get; set; }
        public string Entidade { get; set; }
        public Guid? Id { get; set; }
        public string RF { get; set; }
        public string Usuario { get; set; }
        public Guid? Perfil { get; set; }
        public string Administrador { get; set; }
    }
}
