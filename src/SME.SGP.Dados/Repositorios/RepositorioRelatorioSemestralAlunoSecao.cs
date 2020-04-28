using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralAlunoSecao : IRepositorioRelatorioSemestralAlunoSecao
    {
        private readonly ISgpContext database;

        public RepositorioRelatorioSemestralAlunoSecao(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

    }
}