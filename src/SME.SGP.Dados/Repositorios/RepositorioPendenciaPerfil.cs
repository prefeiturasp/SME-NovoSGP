using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaPerfil : RepositorioBase<PendenciaPerfil>, IRepositorioPendenciaPerfil
    {
        public RepositorioPendenciaPerfil(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PendenciaPerfil>> ObterPorPendenciaId(long pendenciaId)
        {
            var query = @"select pp.*, ppu.* 
                          from pendencia_perfil pp
                          left join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id
                         where pendencia_id = @pendenciaId";

            var lookup = new Dictionary<long, PendenciaPerfil>();

            await database.Conexao.QueryAsync<PendenciaPerfil, PendenciaPerfilUsuario, PendenciaPerfil>(query
                , (perfil, usuario) => 
                {
                    PendenciaPerfil perfilRetorno;
                    if (!lookup.TryGetValue(perfil.Id, out perfilRetorno))
                    {
                        perfilRetorno = perfil;
                        lookup.Add(perfil.Id, perfilRetorno);
                    }

                    perfilRetorno.AdicionaPendenciaPerfilUsuario(usuario);

                    return perfilRetorno;
                }
                , new { pendenciaId });

            return lookup.Values;
        }

        public async Task<bool> Excluir(long id)
        {
            var query = "delete from pendencia_perfil where pendencia_id  = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }
    }
}
