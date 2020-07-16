using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoAluno : RepositorioBase<ComunicadoAluno>, IRepositorioComunicadoAluno
    {
        public RepositorioComunicadoAluno(ISgpContext context) : base(context)
        {

        }

        public async Task<IEnumerable<ComunicadoAluno>> ObterPorComunicado(long comunicadoId)
        {
            var where = "where comunicado_id = @comunicadoId";

            return await database.Conexao.QueryAsync<ComunicadoAluno>($"{Cabecalho} {where}", new { comunicadoId });
        }

        public override void Remover(ComunicadoAluno entidade)
        {
            throw new NegocioException("Não é possivel remover alunos do comunicado");
        }

        public override void Remover(long id)
        {
            throw new NegocioException("Não é possivel remover alunos do comunicado");
        }

        private string Cabecalho => @"
                select 
                    id, 
                    aluno_codigo, 
                    comunicado_id, 
                    criado_em, 
                    alterado_em, 
                    criado_por, 
                    alterado_por, 
                    criado_rf, 
                    alterado_rf 
                from 
                    comunicado_aluno";
    }
}
