using System;

namespace SME.SGP.Dominio
{
    public class Auditoria
    {
        public string Acao { get; set; }
        public long Chave { get; set; }
        public DateTime Data { get; set; }
        public string Entidade { get; set; }
        public long Id { get; set; }
        public string RF { get; set; }
        public string Usuario { get; set; }
        public Guid? Perfil { get; set; }
    }
}