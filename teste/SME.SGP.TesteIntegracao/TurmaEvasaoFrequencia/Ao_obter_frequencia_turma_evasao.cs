﻿using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_obter_frequencia_turma_evasao : TesteBase
    {
        public Ao_obter_frequencia_turma_evasao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_abaixo_50_porcento_agrupado_por_dre()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Modalidade.Medio,
                DreCodigo = "-99",
                UeCodigo = "-99"
            };

            var resultados = await useCase.Executar(filtro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(2);
            resultados.FirstOrDefault(c => c.Descricao == "BT").Quantidade.ShouldBe(10);
            resultados.FirstOrDefault(c => c.Descricao == "JT").Quantidade.ShouldBe(3);
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_abaixo_50_porcento_agrupado_por_ue()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                DreCodigo = "1",
                Modalidade = Modalidade.Medio,
                UeCodigo = "-99"
            };

            var resultados = await useCase.Executar(filtro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
            resultados.FirstOrDefault().Quantidade.ShouldBe(10);
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_abaixo_50_porcento_agrupado_por_turma()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                DreCodigo = "1",
                UeCodigo = "1",
                Modalidade = Modalidade.Medio
            };

            var resultados = await useCase.Executar(filtro);            

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
            resultados.FirstOrDefault().Quantidade.ShouldBe(10);
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_sem_presenca_agrupado_por_dre()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                Modalidade = Modalidade.Medio,
                DreCodigo = "-99",
                UeCodigo = "-99"
            };

            var resultados = await useCase.Executar(filtro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(2);
            resultados.FirstOrDefault(c => c.Descricao == "BT").Quantidade.ShouldBe(4);
            resultados.FirstOrDefault(c => c.Descricao == "JT").Quantidade.ShouldBe(2);
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_sem_presenca_agrupado_por_ue()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                DreCodigo = "1",
                Modalidade = Modalidade.Medio,
                UeCodigo = "-99"
            };

            var resultados = await useCase.Executar(filtro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
            resultados.FirstOrDefault().Quantidade.ShouldBe(4);
        }

        [Fact]
        public async Task Deve_obter_apenas_registros_com_quantidade_alunos_sem_presenca_agrupado_por_turma()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoDto()
            {
                AnoLetivo = 2022,
                DreCodigo = "1",
                UeCodigo = "1",
                Modalidade = Modalidade.Medio
            };

            var resultados = await useCase.Executar(filtro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
            resultados.FirstOrDefault().Quantidade.ShouldBe(4);
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE - BT"
            });

            await InserirNaBase(new Dre
            {
                Id = 2,
                CodigoDre = "2",
                Abreviacao = "DRE - JT"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "UE - 1",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Ue
            {
                Id = 2,
                CodigoUe = "2",
                DreId = 2,
                Nome = "UE - 2",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = "7A"
            });

            await InserirNaBase(new Turma
            {
                Id = 2,
                UeId = 2,
                Ano = "1",
                CodigoTurma = "2",
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = "8A"
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        private async Task CriarRegistrosParaConsulta()
        {
            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 1,
                Mes = 2,
                QuantidadeAlunosAbaixo50Porcento = 3,
                QuantidadeAlunos0Porcento = 0
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 2,
                Mes = 2,
                QuantidadeAlunosAbaixo50Porcento = 1,
                QuantidadeAlunos0Porcento = 0
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 1,
                Mes = 3,
                QuantidadeAlunosAbaixo50Porcento = 5,
                QuantidadeAlunos0Porcento = 2
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 2,
                Mes = 3,
                QuantidadeAlunosAbaixo50Porcento = 2,
                QuantidadeAlunos0Porcento = 1
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 1,
                Mes = 4,
                QuantidadeAlunosAbaixo50Porcento = 2,
                QuantidadeAlunos0Porcento = 1
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 2,
                Mes = 4,
                QuantidadeAlunosAbaixo50Porcento = 0,
                QuantidadeAlunos0Porcento = 1
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 1,
                Mes = 5,
                QuantidadeAlunosAbaixo50Porcento = 0,
                QuantidadeAlunos0Porcento = 1
            });

            await InserirNaBase(new FrequenciaTurmaEvasao
            {
                Id = 1,
                TurmaId = 2,
                Mes = 5,
                QuantidadeAlunosAbaixo50Porcento = 0,
                QuantidadeAlunos0Porcento = 0
            });
        }
    }
}
