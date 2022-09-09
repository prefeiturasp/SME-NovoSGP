using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDevolutivas
{
    public class ObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCaseTeste : TesteBase
    {
        public ObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCaseTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
          
        }

        [Fact]
        public async Task Deve_Executar_Limpar_Consolidacao_Devolutivas_Existe_Turma()
        {
            await CriarDadosBasicos();
            var useCase = ServiceProvider.GetService<IObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCase>();

            var dto = new DevolutivaTurmaDTO
            {
                AnoAtual = 2022,
                AnoLetivo = 2022,
                Id = 1,
                TurmaId = "1",
                UeId = 1
            };
            var jsonMensagem = JsonSerializer.Serialize(dto);
            var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));
            retorno.ShouldBeTrue();
        }

        [Fact]
        public async Task Nao_Deve_Executar_Limpar_Consolidacao_Devolutivas_Nao_Existe_Turma()
        {
            var useCase = ServiceProvider.GetService<IObterTurmaCodigoAulaInfantilPorTurmaCodigoUseCase>();

            var dto = new DevolutivaTurmaDTO
            {
                AnoAtual = 2019,
                AnoLetivo = 2019,
                Id = 1,
                TurmaId = "1",
                UeId = 1
            };
            var jsonMensagem = JsonSerializer.Serialize(dto);
            var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));
            retorno.ShouldBeTrue();
        }

        private async Task CriarDadosBasicos()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "1",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "Nome da Ue",
                TipoEscola = TipoEscola.CRPCONV,
                DataAtualizacao = DateTime.Now,
            });
            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");
            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL'");

            await InserirNaBase(new Devolutiva
            {
                Id = 1,
                Descricao = "Atenção nas Crianças",
                CodigoComponenteCurricular = 512,
                PeriodoInicio = DateTime.Now,
                PeriodoFim = DateTime.Now.AddDays(3),
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
            });

            await InserirNaBase(new Turma
            {
                Id = 1,
                CodigoTurma = "1",
                UeId = 1,
                Ano = "1",
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Semestre = 0,
                QuantidadeDuracaoAula = 10,
                TipoTurno = 6,
                DataAtualizacao = DateTime.Now,
                Historica = false,
                EnsinoEspecial = false,
                EtapaEJA = 0,
                DataInicio = DateTime.Now,
                SerieEnsino = "Bercario",
                TipoTurma = TipoTurma.Regular,
                NomeFiltro = "Bercario",
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                AnoLetivo = 2022,
                Nome = "Tipo 1",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Sistema",
                CriadoRF = "1",
                AlteradoRF = "1",
            });

            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                UeId = "1",
                DisciplinaId = "512",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "Sistema",
                Quantidade = 1,
                DataAula = DateTime.Now,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "Sistema",
                Status = EntidadeStatus.Aprovado
            });

            await InserirNaBase(new DiarioBordo
            {
                Id = 1,
                AulaId = 1,
                DevolutivaId = 1,
                Planejamento = "Planejamento para novo ano",
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "Sistema",
                ComponenteCurricularId = 512,
                TurmaId = 1
            });
        }
    }
}
