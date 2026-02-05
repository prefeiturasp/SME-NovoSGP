using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFeriadoCalendario : RepositorioBase<FeriadoCalendario>, IRepositorioFeriadoCalendario
    {
        public RepositorioFeriadoCalendario(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<FeriadoCalendario>> ObterFeriadosCalendario(FiltroFeriadoCalendarioDto filtro)
        {
            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                filtro.Nome = $"%{filtro.Nome.ToUpper()}%";
            }

            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("nome,");
            query.AppendLine("abrangencia,");
            query.AppendLine("data_feriado,");
            query.AppendLine("tipo,");
            query.AppendLine("ativo");
            query.AppendLine("from feriado_calendario");
            query.AppendLine("where excluido = false");

            if (!string.IsNullOrEmpty(filtro.Nome))
                query.AppendLine("and upper(f_unaccent(nome)) LIKE f_unaccent(@Nome)");

            if (filtro.Abrangencia > 0)
                query.AppendLine("and abrangencia = @Abrangencia");

            if (filtro.Tipo > 0)
                query.AppendLine("and tipo = @Tipo");

            if (filtro.Ano > 0)
                query.AppendLine("and EXTRACT(year FROM data_feriado) = @Ano");

            var listaRetorno = await database.Conexao.QueryAsync<FeriadoCalendario>(query.ToString(), new
            {
                filtro.Nome,
                filtro.Abrangencia,
                filtro.Tipo,
                filtro.Ano
            });

            return listaRetorno;
        }

        FeriadoCalendario IRepositorioBase<FeriadoCalendario>.ObterPorId(long id)
        {
            var query = @"select
                            id,
                            nome,
                            abrangencia,
                            data_feriado,
                            tipo,
                            ativo,
                            criado_em,
                            criado_por,
                            alterado_em,
                            alterado_por,
                            criado_rf,
                            alterado_rf,
                            excluido
                            from feriado_calendario
                            where excluido = false
                                and id = @id";

            return database.Conexao.QueryFirstOrDefault<FeriadoCalendario>(query, new { id });
        }

        public bool VerificarRegistroExistente(long id, string nome)
        {
            StringBuilder query = new StringBuilder();

            var nomeMaiusculo = nome.ToUpper().Trim();
            query.AppendLine("select count(*)");
            query.AppendLine("from feriado_calendario");
            query.AppendLine("where upper(nome) = @nomeMaiusculo");
            query.AppendLine("and excluido = false");
            if (id > 0)
            {
                query.AppendLine("and id <> @id");
            }

            return database.Conexao.QueryFirst<int>(query.ToString(), new { id, nomeMaiusculo }) > 0;
        }
    }
}