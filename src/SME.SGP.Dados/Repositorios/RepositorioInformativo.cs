using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInformativo : RepositorioBase<Informativo>, IRepositorioInformativo
    {
        public RepositorioInformativo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<Informativo> ObterInformes(long id)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT inf.id, inf.titulo, inf.texto, inf.data_envio,
                            inf.criado_em, inf.criado_por, inf.alterado_em,
                            inf.alterado_por, inf.criado_rf, inf.alterado_rf,
                            dre.id, dre.nome, dre.abreviacao,
                            ue.id, ue.nome, ue.tipo_escola,
                            inf_p.id, inf_p.informativo_id, inf_p.codigo_perfil
                            FROM informativo inf
                            INNER JOIN informativo_perfil inf_p ON inf_p.informativo_id = inf.id
                            LEFT JOIN dre ON dre.id = inf.dre_id
                            LEFT JOIN ue ON ue.id = inf.ue_id
                            WHERE inf.id = @id");

            var informativo = new Informativo();

            await database.Conexao
                .QueryAsync<Informativo, Dre, Ue, InformativoPerfil, Informativo>(
                    sql.ToString(), (informes, dre, ue, informativoPerfil) =>
                    {
                        if (informativo.Id == 0)
                        {
                            informativo = informes;
                            informativo.Dre = dre;
                            informativo.Ue = ue;
                            informativo.Perfis = new List<InformativoPerfil>();
                        }

                        informativo.Perfis.Add(informativoPerfil);

                        return informativo;
                    }, new { id });

            return informativo;
        }

        public async Task<bool> RemoverAsync(long id)
        {
            var query = @"delete from informativo where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }
    }
}
