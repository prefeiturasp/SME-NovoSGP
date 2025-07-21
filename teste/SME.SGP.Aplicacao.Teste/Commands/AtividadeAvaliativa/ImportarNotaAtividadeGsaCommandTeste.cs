using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class ImportarNotaAtividadeGsaCommandTeste
    {
        [Fact]
        public void Construtor_Deve_Armazenar_Propriedades_Corretamente()
        {
            var dataInclusao = DateTime.Now;
            var dataEntrega = dataInclusao.AddDays(1);

            var dto = new NotaAtividadeGsaDto
            {
                TurmaId = 123,
                ComponenteCurricularId = 456,
                AtividadeGoogleClassroomId = 789,
                StatusGsa = StatusGSA.Criado,
                Nota = 9.5,
                DataInclusao = dataInclusao,
                DataEntregaAvaliacao = dataEntrega,
                CodigoAluno = 101112,
                Titulo = "Atividade de Matemática"
            };

            var command = new ImportarNotaAtividadeGsaCommand(dto);

            Assert.NotNull(command.NotaAtividadeGsaDto);
            Assert.Equal(dto.TurmaId, command.NotaAtividadeGsaDto.TurmaId);
            Assert.Equal(dto.ComponenteCurricularId, command.NotaAtividadeGsaDto.ComponenteCurricularId);
            Assert.Equal(dto.AtividadeGoogleClassroomId, command.NotaAtividadeGsaDto.AtividadeGoogleClassroomId);
            Assert.Equal(dto.StatusGsa, command.NotaAtividadeGsaDto.StatusGsa);
            Assert.Equal(dto.Nota, command.NotaAtividadeGsaDto.Nota);
            Assert.Equal(dto.DataInclusao, command.NotaAtividadeGsaDto.DataInclusao);
            Assert.Equal(dto.DataEntregaAvaliacao, command.NotaAtividadeGsaDto.DataEntregaAvaliacao);
            Assert.Equal(dto.CodigoAluno, command.NotaAtividadeGsaDto.CodigoAluno);
            Assert.Equal(dto.Titulo, command.NotaAtividadeGsaDto.Titulo);
        }
    }
}
