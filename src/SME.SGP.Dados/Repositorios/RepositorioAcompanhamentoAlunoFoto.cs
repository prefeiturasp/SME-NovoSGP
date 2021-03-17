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

        public async Task<AcompanhamentoAlunoFoto> ObterFotoPorCodigo(Guid codigoFoto)
        {
            var query = @"select aaf.* 
                          from acompanhamento_aluno_foto aaf 
                         inner join arquivo a on a.id = aaf.arquivo_id 
                         where not aaf.excluido 
                           and a.codigo = @";

            return await database.Conexao.QueryFirstOrDefaultAsync<AcompanhamentoAlunoFoto>(query, new { codigoFoto });
        }

        public async Task<AcompanhamentoAlunoFoto> ObterFotoPorMiniaturaId(long miniaturaId)
        {
            var query = @"select aaf.* 
                      from acompanhamento_aluno_foto aaf 
                     where not aaf.excluido 
                       and aaf.miniatura_id = @miniaturaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AcompanhamentoAlunoFoto>(query, new { miniaturaId });
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
