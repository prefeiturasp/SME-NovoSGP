using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoRecomendacao : RepositorioBase<ConselhoClasseAlunoRecomendacao>, IRepositorioConselhoClasseAlunoRecomendacao
    {
        public RepositorioConselhoClasseAlunoRecomendacao(ISgpContext database) : base(database)
        {
        }
    }
}
