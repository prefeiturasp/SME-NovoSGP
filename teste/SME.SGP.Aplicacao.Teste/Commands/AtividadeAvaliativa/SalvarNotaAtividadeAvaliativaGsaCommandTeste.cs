using FluentAssertions;
using SME.SGP.Dominio;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class SalvarNotaAtividadeAvaliativaGsaCommandValidatorTeste
    {
        private readonly SalvarNotaAtividadeAvaliativaGsaCommandValidator _validator;

        public SalvarNotaAtividadeAvaliativaGsaCommandValidatorTeste()
        {
            _validator = new SalvarNotaAtividadeAvaliativaGsaCommandValidator();
        }

        private NotaConceito CriarNotaConceitoValida()
        {
            return new NotaConceito
            {
                AlunoId = "123456",
                AtividadeAvaliativaID = 999,
                DisciplinaId = "MAT",
                Nota = 8.0,
                TipoNota = TipoNota.Nota,
                StatusGsa = StatusGSA.Entregue,
                ConceitoId = null,
            };
        }

        private NotaTipoValor CriarNotaTipoValorNota()
        {
            return new NotaTipoValor
            {
                Ativo = true,
                Descricao = "Nota",
                InicioVigencia = DateTime.Now.AddYears(-1),
                FimVigencia = DateTime.Now.AddYears(1),
                TipoNota = TipoNota.Nota
            };
        }

        private NotaTipoValor CriarNotaTipoValorConceito()
        {
            return new NotaTipoValor
            {
                Ativo = true,
                Descricao = "Conceito",
                InicioVigencia = DateTime.Now.AddYears(-1),
                FimVigencia = DateTime.Now.AddYears(1),
                TipoNota = TipoNota.Conceito
            };
        }

        [Fact]
        public void Deve_Validar_Comando_Valido_Com_Nota()
        {
            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: CriarNotaConceitoValida(),
                nota: 8.0,
                statusGsa: StatusGSA.Entregue,
                atividadeId: 100,
                tipoNota: CriarNotaTipoValorNota(),
                codigoAluno: "123456",
                componenteCurricular: "Matemática");

            var resultado = _validator.Validate(comando);

            resultado.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_Validar_Comando_Com_Conceito_Sem_Nota()
        {
            var conceito = CriarNotaConceitoValida();
            conceito.Nota = null;
            conceito.ConceitoId = 5;
            conceito.TipoNota = TipoNota.Conceito;

            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: conceito,
                nota: null,
                statusGsa: StatusGSA.Entregue,
                atividadeId: 200,
                tipoNota: CriarNotaTipoValorConceito(),
                codigoAluno: "654321",
                componenteCurricular: "Ciências");

            var resultado = _validator.Validate(comando);

            resultado.IsValid.Should().BeTrue();
        }

    }
}
