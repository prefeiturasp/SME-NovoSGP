using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAlunoFoto : RepositorioBase<AlunoFoto>, IRepositorioAlunoFoto
    {
        public RepositorioAlunoFoto(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<AlunoFoto> ObterFotoPorAlunoCodigo(string codigoAluno)
        {
            var query = @"select * from aluno_foto af where not af.excluido and af.aluno_codigo = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<AlunoFoto>(query, new { codigoAluno });
        }

        public async Task<MiniaturaFotoDto> ObterFotosPorAlunoCodigo(string alunoCodigo)
        {
            var query = @"select
	                    a2.Codigo,
	                    a.Codigo as CodigoFotoOriginal,
	                    a.Tipo,
	                    a.tipo_conteudo,
	                    a.Nome,
                        af.id as FotoId,
                        af.arquivo_id as ArquivoId,
                        af2.id as MiniaturaId,
                        af2.arquivo_id as MiniaturaArquivoId
                    from
	                    arquivo a
                    inner join aluno_foto af on
	                    af.arquivo_id = a.id
                    inner join aluno_foto af2 on
	                    af.miniatura_id = af2.id
                    inner join arquivo a2 on
	                    a2.id = af2.arquivo_id
                    where
	                    not af.excluido and
	                    af.aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<MiniaturaFotoDto>(query, new { alunoCodigo });
        }
    }
}
