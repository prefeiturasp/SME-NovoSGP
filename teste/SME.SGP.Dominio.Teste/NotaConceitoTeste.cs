using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class NotaConceitoTeste
    {
        public readonly string ProfessorRf = "7";

        [Fact]
        public void Deve_Validar_Conceitos()
        {
            var notaconceito = ObterEntidade();
            var conceitos = ObterConceitos();

            notaconceito.ConceitoId = 5;

            Assert.Throws<NegocioException>(() => notaconceito.ValidarConceitos(conceitos, "João Kleber"));

            notaconceito.ConceitoId = 0;

            Assert.Throws<NegocioException>(() => notaconceito.ValidarConceitos(conceitos, "João Kleber"));

            notaconceito.ConceitoId = 1;

            notaconceito.ValidarConceitos(conceitos, "João Kleber");
        }

        [Fact]
        public void Deve_Validar_Notas()
        {
            var notaconceito = ObterEntidade();
            var parametroNota = ObterNotaParametro();

            notaconceito.Nota = 5.3;

            Assert.Throws<NegocioException>(() => notaconceito.ValidarNota(parametroNota, "João Kleber"));

            notaconceito.Nota = 10.5;

            Assert.Throws<NegocioException>(() => notaconceito.ValidarNota(parametroNota, "João Kleber"));

            notaconceito.Nota = -.5;

            Assert.Throws<NegocioException>(() => notaconceito.ValidarNota(parametroNota, "João Kleber"));

            notaconceito.Nota = 0.5;

            notaconceito.ValidarNota(parametroNota, "João Kleber");
        }

        [Fact]
        public void Deve_Validar_RF_Alterante_Com_Criador()
        {
            var notaconceito = ObterEntidade();

            Assert.Throws<NegocioException>(() => notaconceito.Validar("8"));
            Assert.Throws<NegocioException>(() => notaconceito.Validar("9"));
            Assert.Throws<NegocioException>(() => notaconceito.Validar("10"));
            Assert.Throws<NegocioException>(() => notaconceito.Validar("11"));

            notaconceito.Validar(ProfessorRf);
        }

        private IEnumerable<Conceito> ObterConceitos()
        {
            var conceitos = new List<Conceito>();

            conceitos.Add(new Conceito { Id = 1, Descricao = "Satisfatorio", Aprovado = true, Ativo = true, FimVigencia = DateTime.Today.AddDays(2), InicioVigencia = DateTime.Today, Valor = "S" });
            conceitos.Add(new Conceito { Id = 2, Descricao = "Plenamente Satisfatorio", Aprovado = true, Ativo = true, FimVigencia = DateTime.Today.AddDays(2), InicioVigencia = DateTime.Today, Valor = "PS" });
            conceitos.Add(new Conceito { Id = 3, Descricao = "Não Satisfatorio", Aprovado = false, Ativo = true, FimVigencia = DateTime.Today.AddDays(2), InicioVigencia = DateTime.Today, Valor = "NS" });

            return conceitos;
        }

        private NotaConceito ObterEntidade()
        {
            return new NotaConceito
            {
                AlunoId = "1",
                AtividadeAvaliativaID = 1,
                ConceitoId = 'P',
                Nota = 10,
                TipoNota = TipoNota.Nota,
                Id = 1,
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Teste",
                AlteradoRF = ProfessorRf,
                CriadoEm = DateTime.Now,
                CriadoPor = "Teste",
                CriadoRF = ProfessorRf
            };
        }

        private NotaParametro ObterNotaParametro()
        {
            return new NotaParametro
            {
                Ativo = true,
                FimVigencia = DateTime.Today.AddDays(2),
                Incremento = 0.5,
                InicioVigencia = DateTime.Today,
                Maxima = 10,
                Media = 5,
                Minima = 0
            };
        }
    }
}