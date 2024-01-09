using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Aula.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_excluir_aula_unica : AulaTeste
    {

        public Ao_excluir_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>), typeof(ObterComponenteCurricularPorIdQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Aula - Deve gerar exceção que a aula não foi encontrada")]
        public async Task Aula_nao_encontrada()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();

            var dto = ObterExcluirAulaDto(RecorrenciaAula.AulaUnica);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        [Fact(DisplayName = "Aula - Deve permitir excluir aula única para professor fundamental")]
        public async Task Exclui_aula_unica()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            await CriarFrequencia();

            await CriarCompensacaoAusencia();

            var excluirAulaUseCase = ServiceProvider.GetService<IExcluirAulaUseCase>();

            var excluirAulaDto = ObterExcluirAulaDto(RecorrenciaAula.AulaUnica);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await excluirAulaUseCase.Executar(excluirAulaDto);

            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Excluido.ShouldBe(true);

            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new FiltroIdDto(AULA_ID)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);

            //Essa fila está dentro do processo do ExcluirAulaUseCase e está sendo chamada aqui de forma exclusiva para o teste
            var excluirCompensacaoAusenciaPorAulaIdUseCase = ServiceProvider.GetService<IExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase>();
            await excluirCompensacaoAusenciaPorAulaIdUseCase.Executar(mensagem);

            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Any(a => a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a => !a.Excluido).ShouldBeFalse();

            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Any(a => a.Excluido).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Any(a => !a.Excluido).ShouldBeFalse();
        }

        private async Task CriarFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F,
                CodigoAluno = ALUNO_CODIGO_1,
                NumeroAula = NUMERO_AULA_1,
                RegistroFrequenciaId = 1,
                AulaId = AULA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F,
                CodigoAluno = ALUNO_CODIGO_1,
                NumeroAula = NUMERO_AULA_2,
                RegistroFrequenciaId = 1,
                AulaId = AULA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F,
                CodigoAluno = ALUNO_CODIGO_1,
                NumeroAula = NUMERO_AULA_3,
                RegistroFrequenciaId = 1,
                AulaId = AULA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusencia()
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_2,
                TurmaId = TURMA_ID_1,
                Nome = "Atividade de compensação",
                Descricao = "Breve descrição da atividade de compensação",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                CompensacaoAusenciaId = 1,
                QuantidadeFaltasCompensadas = NUMERO_AULA_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_1,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_2,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_3,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        [Fact]
        public async Task Aula_possui_avaliacao()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            await CriarAtividadeAvaliativaFundamental(DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarPeriodoEscolarEAbertura();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();

            var dto = ObterExcluirAulaDto(RecorrenciaAula.AulaUnica);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        [Fact(DisplayName = "Aula - Deve excluir aula única do infantil obtendo os componentes curriculares agrupados")]
        public async Task Excluir_aula_unica_infantil_componente_agrupado()
        {
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerComponenteInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDescricaoComponenteCurricularPorIdQuery, string>), typeof(ObterDescricaoComponenteCurricularPorIdQueryHandlerCompInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.Services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHandlerInfantilFake), ServiceLifetime.Scoped));
            _collectionFixture.BuildServiceProvider();

            var dataAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new Dre()
            {
                Id = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                DataAtualizacao = dataAtual,
                TipoTurma = TipoTurma.Regular,
                UeId = 1,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                AnoLetivo = dataAtual.Year
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = dataAtual.Year,
                Nome = "Tipo calendario 1",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Situacao = true,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(dataAtual.Year, dataAtual.Month, 1),
                PeriodoFim = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month)),
                CriadoPor = "Sistema",
                CriadoEm = dataAtual,
                CriadoRF = "1"
            });

            await InserirNaBase(new Dominio.Aula()
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "1",
                Quantidade = 1,
                DataAula = dataAtual.Date,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = dataAtual,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            var excluirAulaUseCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            await excluirAulaUseCase.Executar(new ExcluirAulaDto() { AulaId = 1, RecorrenciaAula = RecorrenciaAula.AulaUnica });

            var aulas = ObterTodos<Dominio.Aula>();

            aulas.ShouldNotBeEmpty();
            aulas.SingleOrDefault(a => a.Id == 1).ShouldNotBeNull();
            aulas.Single(a => a.Id == 1).Excluido.ShouldBeTrue();
        }
    }
}
