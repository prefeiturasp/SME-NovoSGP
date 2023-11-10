using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Informe.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informe
{
    public class Ao_executar_notificacao_Informes_usuario : InformesBase
    {
        public Ao_executar_notificacao_Informes_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Informes - Executar notificação informes usuário verificando código notificação")]
        public async Task Ao_executar_notificacao_Informes_usuario_verifica_codigo_notificacao()
        {
            const long ID_CODIGO = 100000002;

            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "Informe 1",
                Texto = "Teste notificação",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_ADM_SME,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IExecutarNotificacaoInformativoUsuarioUseCase>();

            var dto = new NotificacaoInformativoUsuarioFiltro { 
                InformativoId = INFORME_ID_1,
                Titulo = "Informe 1",
                UsuarioRf = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                Mensagem = "Teste notificação",
                DreCodigo = DRE_CODIGO_1,
                UeCodigo = UE_CODIGO_1
            };

            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            await useCase.Executar(mensagem);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            var noticacao = notificacoes.FirstOrDefault();
            noticacao.Codigo.ShouldBe(ID_CODIGO);

        }
    }
}
