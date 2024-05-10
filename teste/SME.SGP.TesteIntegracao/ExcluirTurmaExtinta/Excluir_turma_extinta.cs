using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ExcluirTurmaExtinta.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Excluir_turma_extinta : TesteBase
    {
        private const string DRE_JT = "DRE - JT";
        private const string DRE_JT_108800 = "108800";

        private const string Ue_Maximo_Moura_Codigo = "094765";
        private const string Ue_Maximo_Moura_Nome = "MAXIMO DE MOURA SANTOS, PROF.";

        private const string Turma_4A_ANO4_Nome = "4A - 4º ANO";
        private const string Turma_4A_ANO4_Ano = "4";
        private const string Turma_4A_ANO4_Codigo = "444";

        private const string Turma_5B_ANO5_Nome = "5B - 5º ANO";
        private const string Turma_5B_ANO5_Ano = "5";
        private const string Turma_5B_ANO5_Codigo = "555";

        private const string Turma_6B_ANO6_Nome = "6B - 6º ANO";
        private const string Turma_6B_ANO6_Ano = "6";
        private const string Turma_6B_ANO6_Codigo = "666";

        private const string Turma_8B_ANO8_Nome = "8B - 8º ANO";
        private const string Turma_8B_ANO8_Ano = "8";
        private const string Turma_8B_ANO8_Codigo = "888";

        private const string Turma_9A_ANO9_Nome = "9A - 9º ANO";
        private const string Turma_9A_ANO9_Ano = "9";
        private const string Turma_9A_ANO9_Codigo = "999";

        private const string Turma_9B_ANO9_Nome = "9B - 9º ANO";
        private const string Turma_9B_ANO9_Ano = "9";
        private const string Turma_9B_ANO9_Codigo = "999B";

        private const string Calendario_Teste_2022 = "Calendário Teste 2022";

        public Excluir_turma_extinta(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>), typeof(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_retornar_false_se_nao_informado_turma()
        {
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase>();

            var retorno = await useCase.Executar(new MensagemRabbit(""));

            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_retornar_true_se_informado_turma()
        {
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase>();

            var filtro = new FiltroTurmaCodigoTurmaIdDto() { TurmaCodigo = Turma_4A_ANO4_Codigo, TurmaId = 1 };

            var retorno = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));

            retorno.ShouldBeTrue();
        }

        [Fact]
        public async Task Deve_retornar_true_quando_excluido_uma_turma()
        {
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase>();

            await CriarItensBasicos();

            var filtro = new FiltroTurmaCodigoTurmaIdDto() { TurmaCodigo = Turma_4A_ANO4_Codigo, TurmaId = 1 };

            var retorno = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));

            retorno.ShouldBeTrue();

            var retornoTurma = ObterTodos<Dominio.Turma>();

            retornoTurma.ShouldNotBeEmpty();
            
            retornoTurma.Any(a=> a.CodigoTurma.Equals(Turma_4A_ANO4_Codigo)).ShouldBeFalse();

            retornoTurma.Any(a=> a.CodigoTurma.Equals(Turma_5B_ANO5_Codigo)).ShouldBeTrue();

            retornoTurma.Count().ShouldBe(5);
        }

        [Fact]
        public async Task Deve_retornar_true_quando_excluido_todas_as_turmas()
        {
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase>();

            await CriarItensBasicos();

            var turmas = new List<string>
            {
                Turma_4A_ANO4_Codigo,
                Turma_5B_ANO5_Codigo,
                Turma_6B_ANO6_Codigo,
                Turma_8B_ANO8_Codigo,
                Turma_9A_ANO9_Codigo,
                Turma_9B_ANO9_Codigo
            };

            long turmaId = 1;

            foreach (var item in turmas)
            {
                var filtro = new FiltroTurmaCodigoTurmaIdDto() { TurmaCodigo = item, TurmaId = turmaId };

                var retorno = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));
    
                retorno.ShouldBeTrue();

                turmaId++;
            }

            var retornoTurma = ObterTodos<Dominio.Turma>();

            retornoTurma.ShouldBeEmpty();

            retornoTurma.Any(a => a.CodigoTurma.Equals(Turma_4A_ANO4_Codigo)).ShouldBeFalse();

            retornoTurma.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Deve_retornar_true_quando_chamado_sincronizacao_Institucional_turma()
        {
            var useCase = ServiceProvider.GetService<IExecutarSincronizacaoInstitucionalTurmaTratarUseCase>();

            await CriarItensBasicos();

            var filtro = new MensagemSyncTurmaDto("1", long.Parse(Turma_4A_ANO4_Codigo));

            var retorno = await useCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(filtro)));

            retorno.ShouldBeTrue();

            var retornoTurma = ObterTodos<Dominio.Turma>();

            retornoTurma.ShouldNotBeEmpty();
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_JT_108800,
                Abreviacao = DRE_JT
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = Ue_Maximo_Moura_Codigo,
                DreId = 1,
                Nome = Ue_Maximo_Moura_Nome,
                TipoEscola = TipoEscola.EMEF
            });
            
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_4A_ANO4_Ano,
                CodigoTurma = Turma_4A_ANO4_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_4A_ANO4_Nome
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_5B_ANO5_Ano,
                CodigoTurma = Turma_5B_ANO5_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_5B_ANO5_Nome
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_6B_ANO6_Ano,
                CodigoTurma = Turma_6B_ANO6_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_6B_ANO6_Nome
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_8B_ANO8_Ano,
                CodigoTurma = Turma_8B_ANO8_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_8B_ANO8_Nome
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_9A_ANO9_Ano,
                CodigoTurma = Turma_9A_ANO9_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_9A_ANO9_Nome
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = Turma_9B_ANO9_Ano,
                CodigoTurma = Turma_9B_ANO9_Codigo,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = Turma_9B_ANO9_Nome
            });

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = 2022,
                Nome = Calendario_Teste_2022,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = "",
                Situacao = true,
                Excluido = false
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoInicio = new DateTime(2022, 1, 1),
                PeriodoFim = new DateTime(2022, 1, 12),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });
        }        
    }
}
