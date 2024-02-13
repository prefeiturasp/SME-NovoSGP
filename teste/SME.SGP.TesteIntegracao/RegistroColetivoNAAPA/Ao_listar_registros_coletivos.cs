using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroColetivoNAAPA.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA
{
    public class Ao_listar_registros_coletivos : RegistroColetivoTesteBase
    {
        public Ao_listar_registros_coletivos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        [Fact(DisplayName = "Registro Coletivo - Listar registros coletivos")]
        public async Task Ao_listar_registros_coletivos_naapa()
        {
            await CriaBase();
            await InserirRegistrosColetivos();
           
            var useCase = ServiceProvider.GetService<IObterRegistrosColetivosNAAPAUseCase>();
            var filtro = new FiltroRegistroColetivoDto()
            {
                DreId = DRE_ID_2,
                UeId = UE_ID_2,
                TiposReuniaoId = new long[] { TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_ID, TipoReuniaoConstants.GRUPO_FOCAL_ID },
                DataReuniaoInicio = DateTimeExtension.HorarioBrasilia().Date.AddDays(-1),
                DataReuniaoFim = DateTimeExtension.HorarioBrasilia().Date.AddDays(1)
            };
            var retorno = await useCase.Executar(filtro);
            Validar(retorno);

            filtro = new FiltroRegistroColetivoDto()
            {
                DreId = DRE_ID_2,
                TiposReuniaoId = null,
                DataReuniaoInicio = DateTimeExtension.HorarioBrasilia().Date.AddDays(-1),
                DataReuniaoFim = DateTimeExtension.HorarioBrasilia().Date.AddDays(1)
            };
            retorno = await useCase.Executar(filtro);
            Validar(retorno);
        }

        private void Validar(PaginacaoResultadoDto<RegistroColetivoListagemDto> retorno)
        {
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(2);
            retorno.Items.Count().ShouldBe(2);

            var primeiroRegistro = retorno.Items.FirstOrDefault();
            primeiroRegistro.DataReuniao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(1));
            primeiroRegistro.NomesUe.All(ue => ue.Equals("EMEF UE 2")).ShouldBe(true);
            primeiroRegistro.TipoReuniaoDescricao.ShouldBe(TipoReuniaoConstants.GRUPO_FOCAL_NOME);

            var ultimoRegistro = retorno.Items.LastOrDefault();
            ultimoRegistro.DataReuniao.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            ultimoRegistro.NomesUe.All(ue => new string[] { "EMEF UE 2", "EMEF UE 3", "EMEF Nome da UE" }.Contains(ue)).ShouldBe(true);
            ultimoRegistro.TipoReuniaoDescricao.ShouldBe(TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_NOME);
        }

        private async Task InserirRegistrosColetivos()
        {
            //Registro Coletivo 1
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
                UeId = UE_ID_1,
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

            //Registro Coletivo 2
            await InserirNaBase(new RegistroColetivo()
            {
                DreId = DRE_ID_2,
                TipoReuniaoId = TipoReuniaoConstants.GRUPO_FOCAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date.AddDays(1),
                QuantidadeCuidadores = 5,
                QuantidadeEducadores = 5,
                QuantidadeEducandos = 5,
                QuantidadeParticipantes = 20,
                Descricao = "Registro coletivo",
                Observacao = "Observação",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 2,
                UeId = UE_ID_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }
    }
}
