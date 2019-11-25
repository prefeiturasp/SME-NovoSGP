using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAusenciaAluno : RepositorioBase<RegistroAusenciaAluno>, IRepositorioRegistroAusenciaAluno
    {
        public RepositorioRegistroAusenciaAluno(ISgpContext database) : base(database)
        {
        }

        public bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            var query = @"update
                            registro_ausencia_aluno
                        set
                            excluido = true
                        where
                            registro_frequencia_id = @registroFrequenciaId";

            return database.Conexao.Execute(query, new { registroFrequenciaId }) > 0;
        }
    }
}