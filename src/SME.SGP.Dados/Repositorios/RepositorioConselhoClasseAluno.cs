using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAluno : RepositorioBase<ConselhoClasseAluno>, IRepositorioConselhoClasseAluno
    {
        public RepositorioConselhoClasseAluno(ISgpContext database) : base(database)
        {
        }
    }
}