using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class AcompanhamentoAlunoFoto : EntidadeBase
    {
        public AcompanhamentoAlunoSemestre AcompanhamentoAlunoSemestre { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }

        [Computed]
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }

        [Computed]
        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }

        [Computed]
        public AcompanhamentoAlunoFoto FotoOriginal { get; set; }

        public bool Excluido { get; set; }
    }
}
