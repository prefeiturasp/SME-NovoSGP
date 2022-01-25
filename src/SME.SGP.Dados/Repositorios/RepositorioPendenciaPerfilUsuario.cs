﻿using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaPerfilUsuario : RepositorioBase<PendenciaPerfilUsuario>, IRepositorioPendenciaPerfilUsuario
    {
        public RepositorioPendenciaPerfilUsuario(ISgpContext database) : base(database)
        {}

        public async Task<bool> Excluir(long id)
        {
            var query = "delete from pendencia_perfil_usuario where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }

        public async Task<IEnumerable<PendenciaPerfilUsuarioDto>> ObterPorSituacao(int situacao)
        {
            var query = @"select ppu.id,
                                 ppu.usuario_id as UsuarioId, 
                                 pp.perfil_codigo as PerfilCodigo,
                                 p.id as PendenciaId,
                                 p.ue_id as UeId,
                                 u.rf_codigo as CodigoRf
                            from pendencia_perfil_usuario ppu 
                                join pendencia_perfil pp on ppu.pendencia_perfil_id = pp.id 
                                join pendencia p on p.id = pp.pendencia_id 
                                join usuario u on u.id = ppu.usuario_id 
                          where p.situacao = @situacao and not p.excluido";

            return await database.Conexao.QueryAsync<PendenciaPerfilUsuarioDto>(query, new { situacao });
        }

        public async Task<IEnumerable<long>> VerificaExistencia (long pendenciaPerfilId, long usuarioId)
        {
            var query = @"select id from pendencia_perfil_usuario 
                                    where pendencia_perfil_id = @pendenciaPerfilId and usuario_id = @usuarioId";

            return await database.Conexao.QueryAsync<long>(query, new { pendenciaPerfilId, usuarioId});
        }
    }
}
