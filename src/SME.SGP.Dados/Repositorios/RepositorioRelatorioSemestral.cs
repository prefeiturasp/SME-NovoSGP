using System;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestral : IRepositorioRelatorioSemestral
    {
        private readonly ISgpContext database;

        public RepositorioRelatorioSemestral(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<RelatorioSemestral> ObterPorTurmaCodigoSemestreAsync(string turmaCodigo, int semestre)
        {
            throw new System.NotImplementedException();
        }
    }
}