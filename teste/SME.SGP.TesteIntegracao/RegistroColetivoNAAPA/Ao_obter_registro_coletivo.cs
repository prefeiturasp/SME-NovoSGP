using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
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
    public class Ao_obter_registro_coletivo : RegistroColetivoTesteBase
    {
        private const string DESCRICAO_REGISTRO_COLETIVO = "Registro coletivo";
        private const string OBSERVACAO_REGISTRO_COLETIVO = "Observação";

        public Ao_obter_registro_coletivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        [Fact(DisplayName = "Registro Coletivo - Obter registro coletivo por Id")]
        public async Task Ao_obter_registro_coletivo_naapa_por_id()
        {
            await CriaBase();
            await InserirRegistrosColetivos();
           
            var useCase = ServiceProvider.GetService<IObterRegistroColetivoNAAPAPorIdUseCase>();
            var retorno = await useCase.Executar(1);
            Validar(retorno, 1);

            retorno = await useCase.Executar(2);
            Validar(retorno, 2);
        }

        [Fact(DisplayName = "Registro Coletivo - Obter registro coletivo por Id inexistente")]
        public async Task Ao_obter_registro_coletivo_naapa_inexisternte()
        {
            await CriaBase();
            var useCase = ServiceProvider.GetService<IObterRegistroColetivoNAAPAPorIdUseCase>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(1));
            excecao.Message.ShouldBe(MensagemNegocioRegistroColetivoNAAPA.REGISTRO_COLETIVO_NAO_ENCONTRADO);
        }

        private void Validar(RegistroColetivoCompletoDto retorno, long id)
        {
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(id);
            
            if (id == 1)
            {
                retorno.DataRegistro.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
                retorno.Ues.All(ue => new string[] { "EMEF UE 2", "EMEF UE 3", "EMEF Nome da UE" }.Contains(ue.NomeFormatado)).ShouldBe(true);
                retorno.TipoReuniaoDescricao.ShouldBe(TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_NOME);
            } else
            {
                retorno.DataRegistro.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(1));
                retorno.Ues.All(ue => ue.NomeFormatado.Equals("EMEF UE 2")).ShouldBe(true);
                retorno.TipoReuniaoDescricao.ShouldBe(TipoReuniaoConstants.GRUPO_FOCAL_NOME);
            }
            retorno.Descricao.ShouldBe($"{DESCRICAO_REGISTRO_COLETIVO} {id}");
            retorno.Observacao.ShouldBe($"{OBSERVACAO_REGISTRO_COLETIVO} {id}");
            retorno.QuantidadeCuidadores.ToString().ShouldBe($"{id}");
            retorno.QuantidadeEducadores.ToString().ShouldBe($"{id}");
            retorno.QuantidadeEducandos.ToString().ShouldBe($"{id}");
            retorno.QuantidadeParticipantes.ToString().ShouldBe($"{id * 3}");
            retorno.Anexos.All(a => a.Nome.Equals($"Arquivo Registro Coletivo {id}"));
            retorno.CriadoEm.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            retorno.CriadoPor.ShouldBe(SISTEMA_NOME);
            retorno.CriadoRF.ShouldBe(SISTEMA_CODIGO_RF);
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
                QuantidadeEducadores = 1,
                QuantidadeEducandos = 1,
                QuantidadeParticipantes = 3,
                Descricao = $"{DESCRICAO_REGISTRO_COLETIVO} 1",
                Observacao = $"{OBSERVACAO_REGISTRO_COLETIVO} 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 1,
                UeId = UE_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 1,
                UeId = UE_ID_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 1,
                UeId = UE_ID_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirAnexos(1);

            //Registro Coletivo 2
            await InserirNaBase(new RegistroColetivo()
            {
                DreId = DRE_ID_2,
                TipoReuniaoId = TipoReuniaoConstants.GRUPO_FOCAL_ID,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date.AddDays(1),
                QuantidadeCuidadores = 2,
                QuantidadeEducadores = 2,
                QuantidadeEducandos = 2,
                QuantidadeParticipantes = 6,
                Descricao = $"{DESCRICAO_REGISTRO_COLETIVO} 2",
                Observacao = $"{OBSERVACAO_REGISTRO_COLETIVO} 2",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new RegistroColetivoUe()
            {
                RegistroColetivoId = 2,
                UeId = UE_ID_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirAnexos(2);
        }

        private async Task InserirAnexos(long registroColetivoId)
        {
            await InserirNaBase(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo Registro Coletivo {registroColetivoId}",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.RegistroColetivo
            });

            await InserirNaBase(new RegistroColetivoAnexo()
            {
                RegistroColetivoId = registroColetivoId,
                ArquivoId = registroColetivoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }
    }
}
