using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AcompanhamentoAlunoFoto : EntidadeBase
    {
        public AcompanhamentoAlunoSemestre AcompanhamentoAlunoSemestre { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }

        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }

        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }

        public AcompanhamentoAlunoFoto FotoOriginal { get; set; }

        public bool Excluido { get; set; }
    }
}
