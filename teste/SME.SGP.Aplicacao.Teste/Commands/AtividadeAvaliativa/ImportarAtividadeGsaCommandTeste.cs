using SME.SGP.Infra;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class ImportarAtividadeGsaCommandTeste
    {
        [Fact]
        public void Deve_Criar_Comando_Com_AtividadeGsa_Valida()
        {
            var data = DateTime.Now;
            var atividadeGsaDto = new AtividadeGsaDto
            {
                AtividadeClassroomId = 123,
                Titulo = "Atividade de teste",
                Descricao = "Descrição da atividade",
                DataCriacao = data
            };

            var comando = new ImportarAtividadeGsaCommand(atividadeGsaDto);

            Assert.NotNull(comando);
            Assert.NotNull(comando.AtividadeGsa);
            Assert.Equal(atividadeGsaDto.AtividadeClassroomId, comando.AtividadeGsa.AtividadeClassroomId);
            Assert.Equal(atividadeGsaDto.Titulo, comando.AtividadeGsa.Titulo);
            Assert.Equal(data, comando.AtividadeGsa.DataCriacao);
        }
    }
}
