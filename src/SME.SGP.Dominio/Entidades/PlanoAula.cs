using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PlanoAula : EntidadeBase
    {
        public string Descricao { get; set; }
        public string RecuperacaoAula { get; set; }
        public string LicaoCasa { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }

        public long AulaId { get; set; }
        [Computed]
        public Aula Aula { get; set; }
    }
}
