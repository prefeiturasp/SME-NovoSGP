using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaUsuario
{
    public class Ao_gerar_pendencia_usuario : TesteBase
    {
        public Ao_gerar_pendencia_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Deve_retornar_pendencia_id_de_pendencia_usuario_ja_cadastrada()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await InserirNaBase(new Dominio.Pendencia()
            {
                Tipo = TipoPendencia.DiarioBordo,
                Descricao = "O registro do Diário de Bordo do componente COMPONENTE CURRICULAR da turma TURMA da UE das aulas abaixo está pendente:",
                Titulo = "Aula sem Diario de Bordo registrado",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01)
            });

            await InserirNaBase(new Usuario()
            {
                Id = 1,
                CodigoRf = "7111111",
                Login = "7111111",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Dominio.PendenciaUsuario()
            {
                Id = 1,
                UsuarioId = 1,
                PendenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            var existePendenciaUsuario = await mediator.Send(new ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery(1, 1));

            existePendenciaUsuario.ShouldBe(true);
        }

        [Fact]
        public async Task Nao_Deve_retornar_pendencia_id_de_pendencia_usuario()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            var existePendenciaUsuario = await mediator.Send(new ObterPendenciasUsuarioPorPendenciaUsuarioIdQuery(1, 1));

            existePendenciaUsuario.ShouldBe(false);
        }
    }
}
