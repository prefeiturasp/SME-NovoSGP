using FluentValidation.TestHelper;
using SME.SGP.Infra;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class SalvarAtividadeAvaliativaGsaCommandValidatorTeste
    {
        private readonly SalvarAtividadeAvaliativaGsaCommandValidator _validator;

        public SalvarAtividadeAvaliativaGsaCommandValidatorTeste()
        {
            _validator = new SalvarAtividadeAvaliativaGsaCommandValidator();
        }

        [Fact]
        public void Validator_Deve_Falhar_Quando_Propriedades_Estiverem_Vazias()
        {
            var command = new SalvarAtividadeAvaliativaGsaCommand(
                default, 
                new AtividadeGsaDto
                {
                    UsuarioRf = null,
                    TurmaId = null,
                    ComponenteCurricularId = 0,
                    Titulo = null,
                    Descricao = null,
                    DataCriacao = default,
                    DataAlteracao = null,
                    AtividadeClassroomId = 0
                });

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DataAula);
            result.ShouldHaveValidationErrorFor(c => c.UsuarioRf);
            result.ShouldHaveValidationErrorFor(c => c.TurmaCodigo);
            result.ShouldHaveValidationErrorFor(c => c.ComponenteCurricularId);
            result.ShouldHaveValidationErrorFor(c => c.Titulo);
            result.ShouldHaveValidationErrorFor(c => c.Descricao);
            result.ShouldHaveValidationErrorFor(c => c.DataCriacao);
            result.ShouldHaveValidationErrorFor(c => c.AtividadeClassroomId);
        }

        [Fact]
        public void Validator_Nao_Deve_Falhar_Quando_Propriedades_Estiverem_Preenchidas()
        {
            var command = new SalvarAtividadeAvaliativaGsaCommand(
                DateTime.Today,
                new AtividadeGsaDto
                {
                    UsuarioRf = "123456",
                    TurmaId = "T001",
                    ComponenteCurricularId = 10,
                    Titulo = "Título da Atividade",
                    Descricao = "Descrição da atividade",
                    DataCriacao = DateTime.Today,
                    DataAlteracao = DateTime.Today.AddDays(-1),
                    AtividadeClassroomId = 123
                });

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
