using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseNota : RepositorioBase<ConselhoClasseNota>, IRepositorioConselhoClasseNota
    {
        public RepositorioConselhoClasseNota(ISgpContext database) : base(database)
        {
        }

    }
}