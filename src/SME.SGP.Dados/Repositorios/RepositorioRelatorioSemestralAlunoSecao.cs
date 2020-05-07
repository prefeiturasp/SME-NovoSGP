using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralAlunoSecao : IRepositorioRelatorioSemestralAlunoSecao
    {
        private readonly ISgpContext database;

        public RepositorioRelatorioSemestralAlunoSecao(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task SalvarAsync(RelatorioSemestralAlunoSecao secaoRelatorioAluno)
        {
            if (secaoRelatorioAluno.Id > 0)
                await database.Conexao.UpdateAsync(secaoRelatorioAluno);
            else
                secaoRelatorioAluno.Id = (long)await database.Conexao.InsertAsync(secaoRelatorioAluno);
        }
    }
}