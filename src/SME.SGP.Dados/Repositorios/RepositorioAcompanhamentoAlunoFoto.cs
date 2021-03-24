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
                           and a.codigo = @codigoFoto";

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

        public async Task<IEnumerable<MiniaturaFotoDto>> ObterFotosPorSemestreId(long acompanhamentoSemestreId, int quantidadeFotos)
        {
            var query = @"select a.Codigo, fo.Codigo as CodigoFotoOriginal, a.Tipo, a.tipo_conteudo, fo.Nome
                          from arquivo a
                         inner join acompanhamento_aluno_foto aaf on aaf.arquivo_id = a.id 
                         inner join acompanhamento_aluno_foto aafo on aafo.miniatura_id = aaf.id 
                         inner join arquivo fo on fo.id = aafo.arquivo_id 
                         where not aaf.excluido 
                           and aaf.acompanhamento_aluno_semestre_id = @acompanhamentoSemestreId
                        order by aaf.id
                        limit @quantidadeFotos";

            return await database.Conexao.QueryAsync<MiniaturaFotoDto>(query, new { acompanhamentoSemestreId, quantidadeFotos });
        }
    }
}
