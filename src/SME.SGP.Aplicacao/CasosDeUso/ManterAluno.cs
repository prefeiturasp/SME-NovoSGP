using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ManterAluno : IManterAluno
    {
        private readonly SgpContext database;
        private readonly IRepositorioAluno repositorioAluno;
        private readonly IRepositorioProfessor repositorioProfessor;

        public ManterAluno(IRepositorioAluno repositorioAluno,
                           IRepositorioProfessor repositorioProfessor,
                           SgpContext database)
        {
            this.repositorioAluno = repositorioAluno ?? throw new System.ArgumentNullException(nameof(repositorioAluno));
            this.repositorioProfessor = repositorioProfessor ?? throw new System.ArgumentNullException(nameof(repositorioProfessor));
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public void Salvar(AlunoDto alunoDto)
        {
            Aluno aluno = MapearParaDominio(alunoDto);
            repositorioAluno.Salvar(aluno);
        }

        public void SalvarCriandoProfessor(AlunoDto alunoDto)
        {
            using (var transacao = database.BeginTransaction())
            {
                Aluno aluno = MapearParaDominio(alunoDto);
                var professor = new Professor()
                {
                    Nome = "Novo professor"
                };
                repositorioProfessor.Salvar(professor);
                aluno.ProfessorId = professor.Id;
                repositorioAluno.Salvar(aluno);
                transacao.Commit();
            }
        }

        public IEnumerable<AlunoDto> Listar(int pagina, int tamanho)
        {
            return MapearParaDto(database.Connection().Query<Aluno>($"select * from tabela_aluno limit @tamanho offset @pagina", new { tamanho, pagina }));
        }

        private IEnumerable<AlunoDto> MapearParaDto(IEnumerable<Aluno> alunos)
        {
            return alunos?.Select(a => MapearParaDto(a));
        }

        private AlunoDto MapearParaDto(Aluno a)
        {
            return a == null ? null : new AlunoDto()
            {
                Email=a.Email,
                Id=a.Id,
                Nome=a.Nome,
                ProfessorId=a.ProfessorId
            };
        }

        private static Aluno MapearParaDominio(AlunoDto alunoDto)
        {
            return new Aluno() { Nome = alunoDto.Nome, Email = alunoDto.Email, ProfessorId = alunoDto.ProfessorId };
        }
    }
}