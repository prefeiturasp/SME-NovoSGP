using Moq;
using SME.SGP.Dados.Contexto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Data;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Contexto
{
    public class SgpContextConsultasAbrangenciaTeste
    {
        private readonly Mock<IContextoAplicacao> contextoAplicacao = new();

        [Fact]
        public void Nao_Deve_Criar_Conexao_Ao_Instanciar()
        {
            //Arrange
            var chamadas = 0;

            //Act
            var contexto = new SgpContextConsultasAbrangencia(() => { chamadas++; return Mock.Of<ISgpContextConsultas>(); }, contextoAplicacao.Object);

            //Assert
            Assert.Equal(0, chamadas);
            Assert.False(contexto.ConexaoCriada);
            Assert.Equal(ConnectionState.Closed, contexto.State);
        }

        [Fact]
        public void Deve_Criar_Conexao_Uma_Unica_Vez_No_Primeiro_Acesso()
        {
            //Arrange
            var chamadas = 0;
            var conexao = Mock.Of<IDbConnection>();
            var interno = new Mock<ISgpContextConsultas>();
            interno.SetupGet(c => c.Conexao).Returns(conexao);
            var contexto = new SgpContextConsultasAbrangencia(() => { chamadas++; return interno.Object; }, contextoAplicacao.Object);

            //Act
            var primeira = contexto.Conexao;
            var segunda = contexto.Conexao;

            //Assert
            Assert.Equal(1, chamadas);
            Assert.Same(conexao, primeira);
            Assert.Same(conexao, segunda);
        }

        [Fact]
        public void Dados_Do_Usuario_Nao_Devem_Abrir_Conexao()
        {
            //Arrange
            var chamadas = 0;
            contextoAplicacao.SetupGet(c => c.UsuarioLogado).Returns("1234567");
            contextoAplicacao.SetupGet(c => c.PerfilUsuario).Returns("perfil");
            var contexto = new SgpContextConsultasAbrangencia(() => { chamadas++; return Mock.Of<ISgpContextConsultas>(); }, contextoAplicacao.Object);

            //Act
            var usuario = contexto.UsuarioLogado;
            var perfil = contexto.PerfilUsuario;

            //Assert
            Assert.Equal("1234567", usuario);
            Assert.Equal("perfil", perfil);
            Assert.Equal(0, chamadas);
        }

        [Fact]
        public void Dispose_Sem_Uso_Nao_Deve_Criar_Conexao()
        {
            //Arrange
            var chamadas = 0;
            var contexto = new SgpContextConsultasAbrangencia(() => { chamadas++; return Mock.Of<ISgpContextConsultas>(); }, contextoAplicacao.Object);

            //Act
            contexto.Close();
            contexto.Dispose();

            //Assert
            Assert.Equal(0, chamadas);
        }

        [Fact]
        public void Dispose_Deve_Liberar_Contexto_Interno_Criado()
        {
            //Arrange
            var interno = new Mock<ISgpContextConsultas>();
            var contexto = new SgpContextConsultasAbrangencia(() => interno.Object, contextoAplicacao.Object);
            _ = contexto.Conexao;

            //Act
            contexto.Dispose();
            contexto.Dispose();

            //Assert
            interno.Verify(c => c.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => contexto.Conexao);
        }
    }
}
