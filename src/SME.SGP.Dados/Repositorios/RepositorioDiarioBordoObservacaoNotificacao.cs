using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacaoNotificacao : IRepositorioDiarioBordoObservacaoNotificacao
    {
        private readonly ISgpContext database;

        public RepositorioDiarioBordoObservacaoNotificacao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<DiarioBordoObservacaoNotificacaoUsuarioDto>> ObterPorDiarioBordoObservacaoId(long diarioBordoObservacaoId)
        {
            var query = @"select dbon.id, dbon.observacao_id IdObservacao, 
                            dbon.notificacao_id IdNotificacao, n.usuario_id IdUsuario 
                          from diario_bordo_observacao_notificacao dbon
                          inner join notificacao n on dbon.notificacao_id = n.id  
                          where observacao_id = @diarioBordoObservacaoId";

            return (await database.Conexao.QueryAsync<DiarioBordoObservacaoNotificacaoUsuarioDto>(query, new { diarioBordoObservacaoId }));
        }

        public async Task<IEnumerable<long>> ObterObservacaoPorId(long diarioBordoId)
        {
            var query = "select id from diario_bordo_observacao where diario_bordo_id = @diarioBordoId";

            return await database.Conexao.QueryAsync<long>(query, new { diarioBordoId });
        }

        public async Task<IEnumerable<UsuarioNotificacaoDto>> ObterUsuariosIdNotificadosPorObservacaoId(long observacaoId)
        {
            var query = @"select 
	                        u.id,
	                        u.rf_codigo as CodigoRf
                        from 	
	                        public.diario_bordo_observacao_notificacao dbon 
                        inner join
	                        public.notificacao n 
	                        on dbon.notificacao_id = n.id 
                        inner join
                            public.usuario u
                            on n.usuario_id = u.id
                        where 
	                        dbon.observacao_id = @observacaoId";

            return await database.Conexao.QueryAsync<UsuarioNotificacaoDto>(query, new { observacaoId });
        }

        public async Task Excluir(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.DeleteAsync(notificacao);
        }

        public async Task Salvar(DiarioBordoObservacaoNotificacao notificacao)
        {
            await database.Conexao.InsertAsync(notificacao);
        }

        public async Task<DiarioBordoObservacaoNotificacao> ObterPorObservacaoUsuarioId(long diarioBordoObservacaoId, long usuarioId)
        {
            var query = @"select dbon.id, dbon.observacao_id IdObservacao, 
                            dbon.notificacao_id IdNotificacao, n.usuario_id IdUsuario 
                          from diario_bordo_observacao_notificacao dbon
                          inner join notificacao n on dbon.notificacao_id = n.id  
                          where observacao_id = @diarioBordoObservacaoId and n.usuario_id = @usuarioId";

            return (await database.Conexao.QueryFirstOrDefaultAsync<DiarioBordoObservacaoNotificacao>(query, new { diarioBordoObservacaoId, usuarioId }));
        }
    }
}
