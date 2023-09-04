﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestao : RepositorioBase<Questao>, IRepositorioQuestao
    {
        public RepositorioQuestao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> VerificaObrigatoriedade(long questaoId)
        {
            var query = @"select obrigatorio from questao where id = @questaoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new {questaoId});
        }

        public async Task<IEnumerable<Questao>> ObterQuestoesPorIds(long[] questaoIds)
        {
            var query = @"select * from questao where id = ANY(@questaoIds)";

            return await database.Conexao.QueryAsync<Questao>(query, new { questaoIds });
        }

        public async Task<Questao> ObterPorNomeComponente(string nomeComponente)
        {
            var query = @"select * from questao where nome_componente = @nomeComponente";

            return await database.Conexao.QueryFirstOrDefaultAsync<Questao>(query, new { nomeComponente });
        }

        public async Task<long?> ObterIdQuestaoPorTipoQuestaoParaQuestionario(long idQuestionario, TipoQuestao tipo)
        {
            var query = @"select id from questao where questionario_id = @idQuestionario and tipo = @tipoQuestao";

            return (await database.Conexao.QueryAsync<long?>(query.ToString(), new { idQuestionario, tipoQuestao = (int)tipo })).FirstOrDefault();
        }
    }
}
