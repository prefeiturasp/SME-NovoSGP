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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA
{
    public class Ao_excluir_registro_coletivo : RegistroColetivoTesteBase
    {
        public Ao_excluir_registro_coletivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro Coletivo - Excluir registro coletivo sem anexo")]
        public async Task Ao_excluir_registro_coletivo_sem_anexo()
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

            var useCase = ServiceProvider.GetService<IExcluirRegistroColetivoUseCase>();
            await useCase.Executar(1);
            var registros = ObterTodos<RegistroColetivo>();
            registros.Count().ShouldBe(0);
            var registrosUes = ObterTodos<RegistroColetivoUe>();
            registrosUes.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Registro Coletivo - Excluir registro coletivo com anexo")]
        public async Task Ao_excluir_registro_coletivo_com_anexo()
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

            await InserirNaBase(new RegistroColetivoAnexo()
            {
                RegistroColetivoId = 1,
                ArquivoId = 1,
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

            await InserirNaBase(new RegistroColetivoAnexo()
            {
                RegistroColetivoId = 1,
                ArquivoId = 2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var dto = new RegistroColetivoDto()
            {
                Id = 1,
                DreId = DRE_ID_2,
                UeIds = new List<long>() { UE_ID_2 },
                TipoReuniaoId = TipoReuniaoConstants.GRUPO_FOCAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                QuantidadeCuidadores = 4,
                QuantidadeEducadores = 5,
                QuantidadeEducandos = 6,
                QuantidadeParticipantes = 7,
                Descricao = "Alteração Registro coletivo",
                Observacao = "Alteração Observação",
                Anexos = new List<AnexoDto>()
                {
                    new AnexoDto(){ AnexoId = codigoArquivo1, ArquivoId = 1 }
                }
            };
            var useCase = ServiceProvider.GetService<IExcluirRegistroColetivoUseCase>();
            await useCase.Executar(1);

            var registros = ObterTodos<RegistroColetivo>();
            registros.Count().ShouldBe(0);
            var registrosUes = ObterTodos<RegistroColetivoUe>();
            registrosUes.Count().ShouldBe(0);
            var registrosAnexo = ObterTodos<RegistroColetivoAnexo>();
            registrosAnexo.Count().ShouldBe(0);
            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count().ShouldBe(0);
        }
    }
}
