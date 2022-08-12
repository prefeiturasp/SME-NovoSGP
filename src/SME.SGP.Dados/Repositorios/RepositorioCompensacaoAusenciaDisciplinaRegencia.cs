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
            var query = @"select * from compensacao_ausencia_disciplina_regencia where not excluido and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaDisciplinaRegencia>(query, new { compensacaoId });
        }
        
        public async Task<bool> InserirVarios(IEnumerable<CompensacaoAusenciaDisciplinaRegencia> registros,Usuario usuarioLogado)
        {
            var sql = @"copy compensacao_ausencia_disciplina_regencia (                                         
                                        compensacao_ausencia_id, 
                                        disciplina_id, 
                                        criado_por,                                        
                                        criado_rf,
                                        criado_em)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var compensacao in registros)
                {
                    writer.StartRow();
                    writer.Write(compensacao.CompensacaoAusenciaId, NpgsqlDbType.Bigint);
                    writer.Write(compensacao.DisciplinaId, NpgsqlDbType.Varchar);
                    writer.Write(compensacao.CriadoPor ?? usuarioLogado.Nome);
                    writer.Write(compensacao.CriadoRF ?? usuarioLogado.Login);
                    writer.Write(compensacao.CriadoEm);
                }
                writer.Complete();
            }
            
            return await Task.FromResult(true);
        }
    }
}
