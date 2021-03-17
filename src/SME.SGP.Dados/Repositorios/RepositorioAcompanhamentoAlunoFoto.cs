using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAlunoFoto : RepositorioBase<AcompanhamentoAlunoFoto>, IRepositorioAcompanhamentoAlunoFoto
    {
        public RepositorioAcompanhamentoAlunoFoto(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<Arquivo>> ObterFotosPorSemestreId(long acompanhamentoSemestreId)
        {
            var query = @"select a.*
                          from arquivo a
                         inner join acompanhamento_aluno_foto aaf on aaf.arquivo_id = a.id 
                         where not aaf.excluido 
                           and aaf.acompanhamento_aluno_semestre_id = @acompanhamentoSemestreId";

            return await database.Conexao.QueryAsync<Arquivo>(query, new { acompanhamentoSemestreId });
        }
    }
}
