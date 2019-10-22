using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFeriadoCalendario : RepositorioBase<FeriadoCalendario>, IRepositorioFeriadoCalendario
    {
        public RepositorioFeriadoCalendario(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<FeriadoCalendario> ObterFeriadosCalendario(FiltroFeriadoCalendarioDto filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                filtro.Nome = $"%{filtro.Nome}%";
            }

            StringBuilder query = new StringBuilder();
            query.AppendLine("select ");
            query.AppendLine("  id,	");
            query.AppendLine("  nome, ");
            query.AppendLine("  abrangencia, ");
            query.AppendLine("  data_feriado, ");
            query.AppendLine("  tipo, ");
            query.AppendLine("  ativo ");
            query.AppendLine("from feriado_calendario ");
            query.AppendLine("where excluido = false ");

            if (!string.IsNullOrEmpty(filtro.Nome))
                query.AppendLine("  and nome LIKE @Nome ");
            if (filtro.Abrangencia > 0)
                query.AppendLine("  and abrangencia = @Abrangencia ");
            if (filtro.Tipo > 0)
                query.AppendLine("  and tipo = @Tipo ");

            return database.Conexao.Query<FeriadoCalendario>(query.ToString(), new
            {
                filtro.Nome,
                filtro.Abrangencia,
                filtro.Tipo
            });
        }

        FeriadoCalendario IRepositorioBase<FeriadoCalendario>.ObterPorId(long id)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select ");
            query.AppendLine("  id,	");
            query.AppendLine("  nome, ");
            query.AppendLine("  abrangencia, ");
            query.AppendLine("  data_feriado, ");
            query.AppendLine("  tipo, ");
            query.AppendLine("  ativo, ");
            query.AppendLine("  criado_em, ");
            query.AppendLine("  criado_por, ");
            query.AppendLine("  alterado_em, ");
            query.AppendLine("  alterado_por, ");
            query.AppendLine("  criado_rf, ");
            query.AppendLine("  alterado_rf, ");
            query.AppendLine("  excluido ");
            query.AppendLine("from feriado_calendario ");
            query.AppendLine("where excluido = false ");
            query.AppendLine("and id = @id ");

            return database.Conexao.QueryFirstOrDefault<FeriadoCalendario>(query.ToString(), new { id });
        }

        public bool VerificarRegistroExistente(long id, string nome)
        {
            StringBuilder query = new StringBuilder();

            var nomeMaiusculo = nome.ToUpper().Trim();
            query.AppendLine("select count(*) ");
            query.AppendLine("from feriado_calendario ");
            query.AppendLine("where upper(nome) = @nomeMaiusculo ");
            query.AppendLine("and excluido = false");
            if (id > 0)
            {
                query.AppendLine("and id <> @id");
            }

            return database.Conexao.QueryFirst<int>(query.ToString(), new { id, nomeMaiusculo }) > 0;
        }
    }
}