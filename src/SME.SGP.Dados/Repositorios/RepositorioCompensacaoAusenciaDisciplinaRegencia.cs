using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaDisciplinaRegencia : RepositorioBase<CompensacaoAusenciaDisciplinaRegencia>, IRepositorioCompensacaoAusenciaDisciplinaRegencia
    {
        public RepositorioCompensacaoAusenciaDisciplinaRegencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> ObterPorCompensacao(long compensacaoId)
        {
            const string query = @"select * from compensacao_ausencia_disciplina_regencia where not excluido and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaDisciplinaRegencia>(query, new { compensacaoId });
        }

        public async Task<bool> InserirVarios(IEnumerable<CompensacaoAusenciaDisciplinaRegencia> registros,Usuario usuarioLogado)
        {
            const string sql = @"copy compensacao_ausencia_disciplina_regencia (                                         
                                        compensacao_ausencia_id, 
                                        disciplina_id, 
                                        criado_por,                                        
                                        criado_rf,
                                        criado_em)
                            from
                            stdin (FORMAT binary)";


            await using var writer = await ((NpgsqlConnection)database.Conexao).BeginBinaryImportAsync(sql);
            foreach (var compensacao in registros)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(compensacao.CompensacaoAusenciaId, NpgsqlDbType.Bigint);
                await writer.WriteAsync(compensacao.DisciplinaId, NpgsqlDbType.Varchar);
                await writer.WriteAsync(compensacao.CriadoPor ?? usuarioLogado.Nome);
                await writer.WriteAsync(compensacao.CriadoRF ?? usuarioLogado.Login);
                await writer.WriteAsync(compensacao.CriadoEm);
            }
            await writer.CompleteAsync();

            return true; 
        }
    }
}