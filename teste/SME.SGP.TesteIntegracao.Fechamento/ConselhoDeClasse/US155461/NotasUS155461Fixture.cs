using Dapper;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.US155461
{
    // A coleção exclusiva protege o interceptor Dapper estático usado pelo projeto.
    [CollectionDefinition("NotasUS155461", DisableParallelization = true)]
    public class NotasUS155461Collection : ICollectionFixture<NotasUS155461Fixture> { }

    public class NotasUS155461Fixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer container = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine").WithDatabase("caracterizacao_notas")
            .WithUsername("postgres").WithPassword(Guid.NewGuid().ToString("N")).Build();
        public NpgsqlConnection Conexao { get; private set; }
        public RepositorioConselhoClasseNotaConsulta Conselho { get; private set; }
        public RepositorioFechamentoNotaConsulta Fechamento { get; private set; }

        public async Task InitializeAsync()
        {
            // Mesmo comportamento de timestamp configurado na API e nos testes integrados do SGP.
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            await container.StartAsync();
            Conexao = new NpgsqlConnection(container.GetConnectionString());
            await Conexao.OpenAsync();
            await SqlMapper.ExecuteAsync(Conexao, @"
                create table turma (id bigint primary key, turma_id varchar(30));
                create table periodo_escolar (id bigint primary key, tipo_calendario_id bigint, bimestre int,
                    periodo_inicio timestamp, periodo_fim timestamp);
                create table fechamento_turma (id bigint primary key, turma_id bigint, periodo_escolar_id bigint);
                create table fechamento_turma_disciplina (id bigint primary key, fechamento_turma_id bigint,
                    disciplina_id bigint, excluido boolean default false);
                create table fechamento_aluno (id bigint primary key, fechamento_turma_disciplina_id bigint, aluno_codigo varchar(30));
                create table fechamento_nota (id bigint primary key, fechamento_aluno_id bigint, disciplina_id bigint,
                    conceito_id bigint, nota double precision, excluido boolean default false);
                create table conselho_classe (id bigint primary key, fechamento_turma_id bigint);
                create table conselho_classe_aluno (id bigint primary key, conselho_classe_id bigint, aluno_codigo varchar(30));
                create table conselho_classe_nota (id bigint primary key, conselho_classe_aluno_id bigint,
                    componente_curricular_codigo bigint, conceito_id bigint, nota double precision, excluido boolean default false);
                create table componente_curricular (id bigint primary key, permite_lancamento_nota boolean);
            ");
            var contexto = new Mock<ISgpContextConsultas>();
            contexto.SetupGet(c => c.Conexao).Returns(Conexao);
            var auditoria = Mock.Of<IServicoAuditoria>();
            Conselho = new RepositorioConselhoClasseNotaConsulta(contexto.Object, auditoria);
            Fechamento = new RepositorioFechamentoNotaConsulta(contexto.Object, auditoria);
            DapperExtensionMethods.Init(new ServicoTelemetria(Options.Create(new TelemetriaOptions { Apm = false })));
        }

        public async Task PrepararAsync()
        {
            // Conexao só pode vir do container próprio desta fixture, nunca de UserSecrets.
            await SqlMapper.ExecuteAsync(Conexao, @"
                truncate turma, periodo_escolar, fechamento_turma, fechamento_turma_disciplina,
                    fechamento_aluno, fechamento_nota, conselho_classe, conselho_classe_aluno,
                    conselho_classe_nota, componente_curricular;
                insert into turma values (1,'TESTE-A'),(2,'TESTE-B');
                insert into periodo_escolar values
                    (1,10,1,'2025-02-01','2025-03-31'),(2,10,2,'2025-04-01','2025-06-30'),
                    (3,20,1,'2025-02-01','2025-03-31');
                insert into fechamento_turma values (1,1,1),(2,1,2),(3,1,null),(4,2,1),(5,1,3);
                insert into fechamento_turma_disciplina values
                    (1,1,89,false),(2,1,139,false),(3,2,89,false),(4,3,89,false),
                    (5,4,89,false),(6,5,89,false),(7,1,7,true);
                insert into fechamento_aluno values (1,1,'ALUNO-A'),(2,1,'ALUNO-B'),(3,3,'ALUNO-A'),
                    (4,4,'ALUNO-A'),(5,5,'ALUNO-A'),(6,6,'ALUNO-A'),(7,7,'ALUNO-A');
                insert into componente_curricular values (89,true),(139,true),(138,false),(7,true);
                insert into fechamento_nota values
                    (1,1,89,null,4,false),(2,1,89,null,7,false),(3,1,139,null,0,false),
                    (4,2,89,null,5,false),(5,3,89,2,null,false),(6,4,89,null,6,false),
                    (7,5,89,null,8,false),(8,6,89,null,9,false),(9,7,7,null,3,false),
                    (10,1,138,null,6,false),(11,1,139,null,2,true);
                insert into conselho_classe values (1,1),(2,2),(3,3),(4,4);
                insert into conselho_classe_aluno values (1,1,'ALUNO-A'),(2,2,'ALUNO-A'),
                    (3,3,'ALUNO-A'),(4,4,'ALUNO-A'),(5,1,'ALUNO-B');
                insert into conselho_classe_nota values
                    (101,1,89,null,9,false),(102,1,139,null,null,false),(103,1,138,3,null,false),
                    (104,1,7,null,4,true),(105,1,89,null,1,true),(106,2,89,3,null,false),
                    (107,3,89,null,10,false),(108,4,89,null,0,false),(109,5,89,null,6,false),
                    (110,1,89,null,9,false);
            ");
        }

        public async Task DisposeAsync()
        {
            if (Conexao != null) await Conexao.DisposeAsync();
            await container.DisposeAsync();
        }
    }
}
