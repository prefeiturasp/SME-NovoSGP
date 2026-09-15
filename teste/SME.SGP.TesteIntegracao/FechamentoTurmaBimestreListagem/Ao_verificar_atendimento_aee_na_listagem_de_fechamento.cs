using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.FechamentoTurmaBimestreListagem.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FechamentoTurmaBimestreListagem
{
    /// <summary>
    /// Characterization: ListarFechamentoTurmaBimestreUseCase.RetornaListagemAlunosFechamentoBimestreEspecifico
    /// deve verificar o atendimento AEE de todos os alunos da turma numa única consulta, retornando
    /// EhAtendidoAEE = true apenas para os alunos com plano AEE ativo.
    /// </summary>
    public class Ao_verificar_atendimento_aee_na_listagem_de_fechamento : FechamentoTurmaBimestreListagemTesteBase
    {
        private const string ALUNO_1 = "1111"; // com plano AEE ativo
        private const string ALUNO_2 = "2222"; // com plano AEE ativo
        private const string ALUNO_3 = "3333"; // sem plano AEE

        public Ao_verificar_atendimento_aee_na_listagem_de_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_de_plano_aee_para_toda_a_turma()
        {
            var turma = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, turmaId: turma.Id);
            await CarregarPlanoAEE(id: 2, versaoId: 2, aluno: ALUNO_2, turmaId: turma.Id);

            var alunos = new[]
            {
                CriarAlunoAtivo(ALUNO_1, 1),
                CriarAlunoAtivo(ALUNO_2, 2),
                CriarAlunoAtivo(ALUNO_3, 3)
            };

            var useCase = CriarUseCase();
            var dto = CriarDto(turma, Enumerable.Empty<FechamentoTurmaDisciplina>());
            var periodoAtual = CriarPeriodoEscolar();

            ContadorQueriesTelemetriaFake.Limpar();
            await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, periodoAtual, null, dto);
            var consultasDePlanoAee = ContadorQueriesTelemetriaFake.ContarPorTrecho("from plano_aee");

            consultasDePlanoAee.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_ehAtendidoAEE_true_para_alunos_com_plano_ativo_e_false_para_os_demais()
        {
            var turma = await CarregarTurma();
            await CarregarPlanoAEE(id: 1, versaoId: 1, aluno: ALUNO_1, turmaId: turma.Id);
            await CarregarPlanoAEE(id: 2, versaoId: 2, aluno: ALUNO_2, turmaId: turma.Id);

            var alunos = new[]
            {
                CriarAlunoAtivo(ALUNO_1, 1),
                CriarAlunoAtivo(ALUNO_2, 2),
                CriarAlunoAtivo(ALUNO_3, 3)
            };

            var useCase = CriarUseCase();
            var dto = CriarDto(turma, Enumerable.Empty<FechamentoTurmaDisciplina>());
            var periodoAtual = CriarPeriodoEscolar();

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, periodoAtual, null, dto);

            resultado.First(a => a.CodigoAluno == ALUNO_1).EhAtendidoAEE.ShouldBeTrue();
            resultado.First(a => a.CodigoAluno == ALUNO_2).EhAtendidoAEE.ShouldBeTrue();
            resultado.First(a => a.CodigoAluno == ALUNO_3).EhAtendidoAEE.ShouldBeFalse();
        }

        private ListarFechamentoTurmaBimestreUseCase CriarUseCase()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            return new ListarFechamentoTurmaBimestreUseCase(mediator);
        }

        private ListagemAlunosFechamentoDto CriarDto(Dominio.Turma turma, IEnumerable<FechamentoTurmaDisciplina> fechamentosTurma)
            => new ListagemAlunosFechamentoDto(
                fechamentosTurma,
                turma,
                DISCIPLINA_ID.ToString(),
                CriarComponenteCurricular(),
                new List<PeriodoEscolar> { CriarPeriodoEscolar() },
                CriarUsuarioProfessor(),
                Enumerable.Empty<string>());

        private async Task CarregarPlanoAEE(long id, long versaoId, string aluno, long turmaId)
        {
            var dataCriacao = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dominio.PlanoAEE
            {
                Id = id,
                TurmaId = turmaId,
                AlunoCodigo = aluno,
                AlunoNome = $"Aluno {aluno}",
                AlunoNumero = 1,
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoEm = dataCriacao,
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            await InserirNaBase(new Dominio.PlanoAEEVersao
            {
                Id = versaoId,
                PlanoAEEId = id,
                Numero = 1,
                Excluido = false,
                CriadoEm = dataCriacao,
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });
        }
    }
}
