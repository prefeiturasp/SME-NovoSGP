using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class AlunoFoto : EntidadeBase
    {
        [Computed]
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }
        [Computed]
        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }
        public string AlunoCodigo { get; set; }
        public bool Excluido { get; set; }
    }
}
