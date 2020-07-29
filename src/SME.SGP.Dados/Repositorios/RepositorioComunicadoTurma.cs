using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoTurma : RepositorioBase<ComunicadoTurma>, IRepositorioComunicadoTurma
    {
        public RepositorioComunicadoTurma(ISgpContext sgpContext) : base(sgpContext)
        {

        }

        public async Task<IEnumerable<ComunicadoTurma>> ObterPorComunicado(long comunicadoId)
        {
            var where = "where comunicado_id = @comunicadoId";

            return await database.Conexao.QueryAsync<ComunicadoTurma>($"{Cabecalho} {where}", new { comunicadoId });
        }

        public async Task RemoverTodasTurmasComunicado(long comunicadoId)
        {
            var update = "update comunicado_turma set excluido=true where comunicado_id = @comunicadoId";

            await database.Conexao.ExecuteAsync(update, new { comunicadoId });
        }

        public override void Remover(ComunicadoTurma entidade)
        {
            throw new NegocioException("Não é possivel alterar a(s) turma(s) do comunicado");
        }

        public override void Remover(long id)
        {
            throw new NegocioException("Não é possivel alterar a(s) turma(s) do comunicado");
        }

        private string Cabecalho => @"
                select 
                    id, 
                    turma_codigo, 
                    comunicado_id, 
                    criado_em, 
                    excluido,
                    alterado_em, 
                    criado_por, 
                    alterado_por, 
                    criado_rf, 
                    alterado_rf 
                from 
                    comunicado_turma";
    }
}
