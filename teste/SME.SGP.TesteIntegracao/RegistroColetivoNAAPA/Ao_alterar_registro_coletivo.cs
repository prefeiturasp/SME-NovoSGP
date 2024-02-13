using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroColetivoNAAPA.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA
{
    public class Ao_alterar_registro_coletivo : RegistroColetivoTesteBase
    {
        public Ao_alterar_registro_coletivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        [Fact(DisplayName = "Registro Coletivo - Alterar registro coletivo sem anexo")]
        public async Task Ao_alterar_registro_coletivo_sem_anexo()
        {
            await CriaBase();

             await InserirNaBase(new RegistroColetivo()
            {
                DreId = DRE_ID_2,
                TipoReuniaoId = TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                QuantidadeCuidadores = 1,
                QuantidadeEducadores = 2,
                QuantidadeEducandos = 3,
                QuantidadeParticipantes = 4,
                Descricao = "Registro coletivo",
                Observacao = "Observação",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 1,
                UeId = UE_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 1,
                UeId = UE_ID_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var dto = new RegistroColetivoDto()
            {
                Id = 1,
                DreId = DRE_ID_2,
                UeIds = new List<long>() { UE_ID_2, UE_ID_3 },
                TipoReuniaoId = TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                QuantidadeCuidadores = 1,
                QuantidadeEducadores = 2,
                QuantidadeEducandos = 3,
                QuantidadeParticipantes = 4,
                Descricao = "Registro coletivo",
                Observacao = "Observação"
            };
            var useCase = ServiceProvider.GetService<ISalvarRegistroColetivoUseCase>();
        }
    }
}
