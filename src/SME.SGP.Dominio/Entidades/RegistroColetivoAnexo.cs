using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class RegistroColetivoAnexo : EntidadeBase
    {
        public long RegistroColetivoId { get; set; }
        [Computed]
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }
        public bool Excluido { get; set; }
    }
}
