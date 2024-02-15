using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.RegistroColetivoNAAPA.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA
{
    public class Ao_salvar_registro_coletivo : RegistroColetivoTesteBase
    {
        public Ao_salvar_registro_coletivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro Coletivo - Salvar registro coletivo sem anexo")]
        public async Task Ao_salvar_registro_coletivo_sem_anexo()
        {
            await CriaBase();

            var dto = new RegistroColetivoDto()
            {
                DreId = DRE_ID_2,
                UeIds = new List<long>() { UE_ID_2, UE_ID_3 },
                TipoReuniaoId = TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                QuantidadeCuidadores = 1,
                QuantidadeEducadores = 2,
                QuantidadeEducandos = 3,
                QuantidadeParticipantes = 4,
                Descricao = "Registro coletivo",
                Observacao = "Observação",
            };
            var useCase = ServiceProvider.GetService<ISalvarRegistroColetivoUseCase>();

            var resultado = await useCase.Executar(dto);
            resultado.ShouldNotBeNull();

            var registros = ObterTodos<RegistroColetivo>();
            registros.ShouldNotBeNull();
            var registro = registros.FirstOrDefault();
            registro.ShouldNotBeNull();
            registro.DreId.ShouldBe(dto.DreId);
            registro.TipoReuniaoId.ShouldBe(dto.TipoReuniaoId);
            registro.DataRegistro.ShouldBe(dto.DataRegistro);
            registro.QuantidadeCuidadores.ShouldBe(dto.QuantidadeCuidadores);
            registro.QuantidadeEducadores.ShouldBe(dto.QuantidadeEducadores);
            registro.QuantidadeEducandos.ShouldBe(dto.QuantidadeEducandos);
            registro.QuantidadeParticipantes.ShouldBe(dto.QuantidadeParticipantes);
            registro.Descricao.ShouldBe(dto.Descricao);
            registro.Observacao.ShouldBe(dto.Observacao);

            var registrosUes = ObterTodos<RegistroColetivoUe>().FindAll(regitro => regitro.RegistroColetivoId == 1);
            registrosUes.ShouldNotBeNull();
            registrosUes.Count().ShouldBe(2);
            registrosUes.Exists(regitro => regitro.UeId == UE_ID_2).ShouldBeTrue();
            registrosUes.Exists(regitro => regitro.UeId == UE_ID_3).ShouldBeTrue();
        }

        [Fact(DisplayName = "Registro Coletivo - Salvar registro coletivo com anexo")]
        public async Task Ao_salvar_registro_coletivo_com_anexo()
        {
            await CriaBase();

            var codigoArquivo1 = Guid.NewGuid();
            await InserirNaBase(new Arquivo()
            {
                Codigo = codigoArquivo1,
                Nome = $"Arquivo - 1",
                Tipo = TipoArquivo.Geral,
                TipoConteudo = "application/pdf",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var codigoArquivo2 = Guid.NewGuid();
            await InserirNaBase(new Arquivo()
            {
                Codigo = codigoArquivo2,
                Nome = $"Arquivo - 2",
                Tipo = TipoArquivo.Geral,
                TipoConteudo = "application/pdf",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var dto = new RegistroColetivoDto()
            {
                DreId = DRE_ID_2,
                UeIds = new List<long>() { UE_ID_2, UE_ID_3 },
                TipoReuniaoId = TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                QuantidadeCuidadores = 1,
                QuantidadeEducadores = 2,
                QuantidadeEducandos = 3,
                QuantidadeParticipantes = 4,
                Descricao = "Registro coletivo",
                Observacao = "Observação",
                Anexos = new List<AnexoDto>() 
                { 
                    new AnexoDto(){ AnexoId = codigoArquivo1 },
                    new AnexoDto(){ AnexoId = codigoArquivo2 }
                }
            };
            var useCase = ServiceProvider.GetService<ISalvarRegistroColetivoUseCase>();

            var resultado = await useCase.Executar(dto);
            resultado.ShouldNotBeNull();

            var registros = ObterTodos<RegistroColetivo>();
            registros.ShouldNotBeNull();
            var registro = registros.FirstOrDefault();
            registro.ShouldNotBeNull();
            registro.DreId.ShouldBe(dto.DreId);
            registro.TipoReuniaoId.ShouldBe(dto.TipoReuniaoId);
            registro.DataRegistro.ShouldBe(dto.DataRegistro);
            registro.QuantidadeCuidadores.ShouldBe(dto.QuantidadeCuidadores);
            registro.QuantidadeEducadores.ShouldBe(dto.QuantidadeEducadores);
            registro.QuantidadeEducandos.ShouldBe(dto.QuantidadeEducandos);
            registro.QuantidadeParticipantes.ShouldBe(dto.QuantidadeParticipantes);
            registro.Descricao.ShouldBe(dto.Descricao);
            registro.Observacao.ShouldBe(dto.Observacao);

            var registrosUes = ObterTodos<RegistroColetivoUe>().FindAll(regitro => regitro.RegistroColetivoId == 1);
            registrosUes.ShouldNotBeNull();
            registrosUes.Count().ShouldBe(2);
            registrosUes.Exists(regitro => regitro.UeId == UE_ID_2).ShouldBeTrue();
            registrosUes.Exists(regitro => regitro.UeId == UE_ID_3).ShouldBeTrue();

            var registrosAnexo = ObterTodos<RegistroColetivoAnexo>().FindAll(regitro => regitro.RegistroColetivoId == 1);
            registrosAnexo.ShouldNotBeNull();
            registrosAnexo.Count().ShouldBe(2);
            registrosAnexo.Exists(regitro => regitro.ArquivoId == 1).ShouldBeTrue();
            registrosAnexo.Exists(regitro => regitro.ArquivoId == 2).ShouldBeTrue();
        }
    }
}
