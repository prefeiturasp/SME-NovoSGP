using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<CompensacaoAusencia>> Listar(string DisciplinaId, int Bimestre, string NomeAtividade)
        {
            throw new NotImplementedException();
        }
    }
}
