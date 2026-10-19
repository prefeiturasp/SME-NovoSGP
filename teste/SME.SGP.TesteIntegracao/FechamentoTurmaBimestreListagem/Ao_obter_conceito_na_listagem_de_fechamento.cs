using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.FechamentoTurmaBimestreListagem.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FechamentoTurmaBimestreListagem
{
    /// <summary>
    /// Characterization: ListarFechamentoTurmaBimestreUseCase deve buscar todos os Conceitos
    /// referenciados pelas notas do bimestre numa única consulta em lote. O valor retornado é o
    /// próprio Id do conceito (não Conceito.Valor) quando ele existe, ou 0 quando não existe/não é
    /// encontrado.
    /// </summary>
    public class Ao_obter_conceito_na_listagem_de_fechamento : FechamentoTurmaBimestreListagemTesteBase
    {
        private const string ALUNO_1 = "1111";
        private const string ALUNO_2 = "2222";
        private const long CONCEITO_ORFAO = 999999; // não existe na tabela conceito

        public Ao_obter_conceito_na_listagem_de_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_getasync_para_todas_as_notas_com_conceito()
        {
            var turma = await CarregarTurma();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Bom", ativo: true));

            var fechamentoTurmaDisciplina = await CarregarFechamento(new[] { ALUNO_1, ALUNO_2 });
            // Duas notas por conceito no total (uma por aluno).
            await CarregarNotaConceito(fechamentoAlunoId: 1, conceitoId: conceitoId);
            await CarregarNotaConceito(fechamentoAlunoId: 2, conceitoId: conceitoId);

            var alunos = new[] { CriarAlunoAtivo(ALUNO_1, 1), CriarAlunoAtivo(ALUNO_2, 2) };
            var useCase = CriarUseCase();
            var dto = CriarDto(turma, new[] { fechamentoTurmaDisciplina });

            ContadorQueriesTelemetriaFake.Limpar();
            await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, CriarPeriodoEscolar(), null, dto);
            var buscasDeConceito = ContadorQueriesTelemetriaFake.ContarPorTrecho("from conceito_valores");

            buscasDeConceito.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_o_proprio_id_como_notaconceito_quando_conceito_existe()
        {
            var turma = await CarregarTurma();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Bom", ativo: true));

            var fechamentoTurmaDisciplina = await CarregarFechamento(new[] { ALUNO_1 });
            await CarregarNotaConceito(fechamentoAlunoId: 1, conceitoId: conceitoId);

            var alunos = new[] { CriarAlunoAtivo(ALUNO_1, 1) };
            var useCase = CriarUseCase();
            var dto = CriarDto(turma, new[] { fechamentoTurmaDisciplina });

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, CriarPeriodoEscolar(), null, dto);

            var notaConceito = resultado.Single().NotasConceitoBimestre.Single();
            notaConceito.NotaConceito.ShouldBe((double)conceitoId);
        }

        [Fact]
        public async Task Deve_retornar_zero_quando_conceito_referenciado_nao_existe_mais()
        {
            var turma = await CarregarTurma();
            // Propositalmente não insere nenhum Conceito com id = CONCEITO_ORFAO.

            var fechamentoTurmaDisciplina = await CarregarFechamento(new[] { ALUNO_1 });
            await CarregarNotaConceito(fechamentoAlunoId: 1, conceitoId: CONCEITO_ORFAO);

            var alunos = new[] { CriarAlunoAtivo(ALUNO_1, 1) };
            var useCase = CriarUseCase();
            var dto = CriarDto(turma, new[] { fechamentoTurmaDisciplina });

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, CriarPeriodoEscolar(), null, dto);

            var notaConceito = resultado.Single().NotasConceitoBimestre.Single();
            notaConceito.NotaConceito.ShouldBe(0d);
        }

        [Fact]
        public async Task Deve_retornar_valor_do_conceito_mesmo_quando_esta_inativo()
        {
            var turma = await CarregarTurma();
            var conceitoId = await InserirNaBaseAsync(CriarConceito("Descontinuado", ativo: false));

            var fechamentoTurmaDisciplina = await CarregarFechamento(new[] { ALUNO_1 });
            await CarregarNotaConceito(fechamentoAlunoId: 1, conceitoId: conceitoId);

            var alunos = new[] { CriarAlunoAtivo(ALUNO_1, 1) };
            var useCase = CriarUseCase();
            var dto = CriarDto(turma, new[] { fechamentoTurmaDisciplina });

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, CriarPeriodoEscolar(), null, dto);

            var notaConceito = resultado.Single().NotasConceitoBimestre.Single();
            notaConceito.NotaConceito.ShouldBe((double)conceitoId);
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_getasync_mesmo_com_conceitos_distintos()
        {
            var turma = await CarregarTurma();
            var conceitoId1 = await InserirNaBaseAsync(CriarConceito("Bom", ativo: true));
            var conceitoId2 = await InserirNaBaseAsync(CriarConceito("Ruim", ativo: true));

            var fechamentoTurmaDisciplina = await CarregarFechamento(new[] { ALUNO_1, ALUNO_2 });
            await CarregarNotaConceito(fechamentoAlunoId: 1, conceitoId: conceitoId1);
            await CarregarNotaConceito(fechamentoAlunoId: 2, conceitoId: conceitoId2);

            var alunos = new[] { CriarAlunoAtivo(ALUNO_1, 1), CriarAlunoAtivo(ALUNO_2, 2) };
            var useCase = CriarUseCase();
            var dto = CriarDto(turma, new[] { fechamentoTurmaDisciplina });

            ContadorQueriesTelemetriaFake.Limpar();
            await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, CriarPeriodoEscolar(), null, dto);
            var buscasDeConceito = ContadorQueriesTelemetriaFake.ContarPorTrecho("from conceito_valores");

            buscasDeConceito.ShouldBe(1);
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

        private Dominio.Conceito CriarConceito(string valor, bool ativo)
            => new Dominio.Conceito
            {
                Descricao = valor,
                Valor = valor,
                Aprovado = true,
                Ativo = ativo,
                InicioVigencia = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                FimVigencia = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            };

        private async Task CarregarNotaConceito(long fechamentoAlunoId, long conceitoId)
        {
            await InserirNaBase(new FechamentoNota
            {
                Id = fechamentoAlunoId,
                FechamentoAlunoId = fechamentoAlunoId,
                DisciplinaId = DISCIPLINA_ID,
                ConceitoId = conceitoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });
        }
    }
}
