using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasse: RepositorioBase<ConselhoClasse>, IRepositorioConselhoClasse
    {
        public RepositorioConselhoClasse(ISgpContext database): base(database)
        {
        }
    }
}
