using Dapper;
using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAluno : RepositorioBase<Aluno>, IRepositorioAluno
    {
        public RepositorioAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public override IEnumerable<Aluno> Listar()
        {
            return database.Conexao().Query<Aluno>("select * from tabela_aluno where");
        }

        public IEnumerable<Aluno> ListarDommel()
        {
            return database.Conexao().GetAll<Aluno, Professor, Aluno>((aluno, professor) =>
            {
                aluno.Professor = professor;
                return aluno;
            });
        }

        //public override void Salvar(Aluno entidade)
        //{
        //    database.Connection().Execute(@"insert into tabela_aluno (Email, nome_aluno, ProfessorId) values (@Email, @Nome, @ProfessorId)", entidade);
        //}
    }
}