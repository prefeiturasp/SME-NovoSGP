﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoObservacao : RepositorioBase<DiarioBordoObservacao>, IRepositorioDiarioBordoObservacao
    {
        public RepositorioDiarioBordoObservacao(ISgpContext conexao) : base(conexao) { }

        public async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> ListarPorDiarioBordoAsync(long diarioBordoId, long usuarioLogadoId)
        {
            var sql = @"select
							dbo.id,
							observacao,
							(usuario_id = @usuarioLogadoId) as Proprietario,
							count(dbon.*) as QtdUsuariosNotificados,
							criado_em as CriadoEm,
							criado_por as CriadoPor,
							criado_rf as CriadoRf,
							alterado_em as AlteradoEm,
							alterado_por as AlteradoPor,
							alterado_rf as AlteradoRf
						from
							diario_bordo_observacao dbo
						left join diario_bordo_observacao_notificacao dbon on dbo.id = dbon.observacao_id
						where
							diario_bordo_id = @diarioBordoId
							and not excluido 
						group by dbo.id,
							observacao,
							usuario_id,
							criado_em,
							criado_por,
							criado_rf,
							alterado_em,
							alterado_por,
							alterado_rf
                        order by criado_em desc";

            return await database.Conexao.QueryAsync<ListarObservacaoDiarioBordoDto>(sql, new { diarioBordoId, usuarioLogadoId });
        }
    }
}
