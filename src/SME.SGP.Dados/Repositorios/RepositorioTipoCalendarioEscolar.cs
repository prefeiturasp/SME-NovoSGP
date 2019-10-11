using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dto;
using System.Collections.Generic;
using Dapper;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoCalendarioEscolar : RepositorioBase<TipoCalendarioEscolar>, IRepositorioTipoCalendarioEscolar
    {
        public RepositorioTipoCalendarioEscolar(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<TipoCalendarioEscolarDto> ObterTiposCalendarioEscolar()
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select ");
            query.AppendLine("id, ");
            query.AppendLine("nome, ");
            query.AppendLine("ano_letivo, ");
            query.AppendLine("periodo ");
            query.AppendLine("from tipo_calendario_escolar ");
            query.AppendLine("where excluido = false");

            return database.Conexao.Query<TipoCalendarioEscolarDto>(query.ToString());
        }

        public bool VerificarRegistroExistente(int id, string nome)
        {
            StringBuilder query = new StringBuilder();

            var nomeMaiusculo = nome.ToUpper().Trim();
            query.AppendLine("select count(*) ");
            query.AppendLine("from tipo_calendario_escolar ");
            query.AppendLine("where upper(nome) = @nomeMaiusculo ");
            query.AppendLine("and excluido = false");
            if (id > 0)
            {
                query.AppendLine("and id <> @id");
            }

            int quantidadeRegistrosExistentes = database.Conexao.QueryFirst<int>(query.ToString(), new { id, nomeMaiusculo }); ;

            return quantidadeRegistrosExistentes > 0;
        }
        public override TipoCalendarioEscolar ObterPorId(long id)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select * ");
            query.AppendLine("from tipo_calendario_escolar ");
            query.AppendLine("where excluido = false ");
            query.AppendLine("and id = @id ");

            return database.Conexao.QueryFirstOrDefault<TipoCalendarioEscolar>(query.ToString(), new { id });
        }
    }
}
