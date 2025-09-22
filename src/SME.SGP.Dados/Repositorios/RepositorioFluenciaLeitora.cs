using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFluenciaLeitora : RepositorioBase<FluenciaLeitora>, IRepositorioFluenciaLeitora
    {
        public RepositorioFluenciaLeitora(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<FluenciaLeitora> ObterRegistroFluenciaLeitoraAsync(int anoLetivo, string codigoEOLTurma, string codigoEOLAluno, int tipoAvaliacao)
        {
            string query = @"select * from fluencia_leitora
                            where ano_letivo = @anoLetivo
                            and codigo_eol_turma = @codigoEOLTurma
                            and codigo_eol_aluno = @codigoEOLAluno
                            and tipo_avaliacao = @tipoAvaliacao;";

            return database.Conexao.QueryFirstOrDefault<FluenciaLeitora>(query, new
            {
                anoLetivo,
                codigoEOLTurma,
                codigoEOLAluno,
                tipoAvaliacao
            });
        }
    }
}
