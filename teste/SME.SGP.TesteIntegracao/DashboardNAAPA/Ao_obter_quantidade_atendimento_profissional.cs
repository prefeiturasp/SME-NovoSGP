using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardNAAPA
{
    public class Ao_obter_quantidade_atendimento_profissional : TesteBaseComuns
    {
        public Ao_obter_quantidade_atendimento_profissional(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_por_ue()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoAtendimentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Quantidade = 4,
                Mes = 2,
                NomeProfissional = USUARIO_PROFESSOR_NOME_1111111,
                RfProfissional = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoAtendimentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Quantidade = 4,
                Mes = 3,
                NomeProfissional = USUARIO_PROFESSOR_NOME_1111111,
                RfProfissional = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = DRE_ID_1,UeId = UE_ID_1  };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.Graficos.FirstOrDefault().Quantidade.ShouldBe(8);
        }

        [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_por_ue_mes()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoAtendimentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Quantidade = 4,
                Mes = 2,
                NomeProfissional = USUARIO_PROFESSOR_NOME_1111111,
                RfProfissional = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoAtendimentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Quantidade = 4,
                Mes = 3,
                NomeProfissional = USUARIO_PROFESSOR_NOME_2222222,
                RfProfissional = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeAtendimentoNAAPAPorProfissionalMesUseCase>();
            var dto = new FiltroQuantidadeAtendimentoNAAPAPorProfissionalMesDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = DRE_ID_1, UeId = UE_ID_1, Mes = 3 };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.Graficos.Count().ShouldBe(1);
            var grafico = retorno.Graficos.FirstOrDefault();
            grafico.Quantidade.ShouldBe(4);
            grafico.Descricao.ShouldBe(USUARIO_PROFESSOR_NOME_2222222);
        }
    }
}
