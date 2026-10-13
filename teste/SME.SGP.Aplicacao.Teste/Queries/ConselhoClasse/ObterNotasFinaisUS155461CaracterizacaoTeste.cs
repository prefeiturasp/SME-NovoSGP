using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ConselhoClasse
{
    // Caracteriza o Handle atual. Não estabelece novas regras de precedência ou de turma.
    public class ObterNotasFinaisUS155461CaracterizacaoTeste
    {
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();

        [Fact]
        public async Task Deve_preservar_fechamento_quando_nao_ha_conselho()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, 2, null, 2), Nota(139, null, 0) };
            var retorno = await Executar(fechamento, new NotaConceitoBimestreComponenteDto[0]);
            Comparar(fechamento, retorno);
        }

        [Fact]
        public async Task Deve_preferir_conselho_preenchido_e_preservar_outros_componentes_e_periodos()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, 2, 6), Nota(139, 1, 5) };
            var conselho = Nota(89, 1, 9, id: 101);
            var retorno = await Executar(fechamento, new[] { conselho });
            Comparar(new[] { conselho, fechamento[1], fechamento[2] }, retorno);
        }

        [Theory]
        [InlineData(0.0, null)]
        [InlineData(null, 3L)]
        public async Task Deve_considerar_zero_e_conceito_como_valores_preenchidos(double? valor, long? conceito)
        {
            var conselho = Nota(89, 1, valor, conceito, 101);
            Comparar(new[] { conselho }, await Executar(new[] { Nota(89, 1, 7) }, new[] { conselho }));
        }

        [Fact]
        public async Task Deve_usar_fechamento_quando_conselho_nao_tem_nota_nem_conceito()
        {
            var fechamento = Nota(89, 1, 7);
            Comparar(new[] { fechamento }, await Executar(new[] { fechamento }, new[] { Nota(89, 1, null, id: 101) }));
        }

        [Fact]
        public async Task Deve_preservar_nota_existente_somente_no_conselho()
        {
            var conselho = Nota(139, null, 8, id: 101);
            Comparar(new[] { conselho }, await Executar(new NotaConceitoBimestreComponenteDto[0], new[] { conselho }));
        }

        [Fact]
        public async Task Deve_preservar_registros_distintos_do_conselho_sem_deduplicar_valores()
        {
            var conselho = new[] { Nota(89, 1, 9, id: 101), Nota(89, 1, 9, id: 102) };
            Comparar(conselho, await Executar(new[] { Nota(89, 1, 7) }, conselho));
        }

        [Fact]
        public async Task Deve_caracterizar_comparacao_por_componente_e_bimestre_entre_turmas()
        {
            var fechamento = Nota(89, 1, 7);
            var conselho = Nota(89, 1, 9, id: 101);
            conselho.TurmaCodigo = "TESTE-B";
            Comparar(new[] { conselho }, await Executar(new[] { fechamento }, new[] { conselho }));
        }

        [Fact]
        public async Task Deve_tratar_periodo_final_nulo_separadamente_do_bimestre()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, null, 6) };
            var conselho = Nota(89, null, 10, id: 101);
            Comparar(new[] { conselho, fechamento[0] }, await Executar(fechamento, new[] { conselho }));
        }

        private static void Comparar(IEnumerable<NotaConceitoBimestreComponenteDto> esperado,
            IEnumerable<NotaConceitoBimestreComponenteDto> atual)
        {
            // Verifica dados e multiplicidade, sem exigir identidade dos objetos ou ordem não contratada.
            Assert.Equal(esperado.Select(n => JsonConvert.SerializeObject(n)).OrderBy(n => n, StringComparer.Ordinal),
                atual.Select(n => JsonConvert.SerializeObject(n)).OrderBy(n => n, StringComparer.Ordinal));
        }

        private async Task<NotaConceitoBimestreComponenteDto[]> Executar(
            IEnumerable<NotaConceitoBimestreComponenteDto> fechamento, IEnumerable<NotaConceitoBimestreComponenteDto> conselho)
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamento);
            mediator.Setup(m => m.Send(It.IsAny<ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(conselho);
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(mediator.Object);
            return (await handler.Handle(new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A", "TESTE-B" }, "ALUNO-A"), CancellationToken.None)).ToArray();
        }

        private static NotaConceitoBimestreComponenteDto Nota(long componente, int? bimestre, double? nota,
            long? conceito = null, long id = 0)
            => new NotaConceitoBimestreComponenteDto
            {
                AlunoCodigo = "ALUNO-A",
                TurmaCodigo = "TESTE-A",
                ComponenteCurricularCodigo = componente,
                Bimestre = bimestre,
                Nota = nota,
                ConceitoId = conceito,
                ConselhoClasseNotaId = id,
                ConselhoClasseId = id > 0 ? 1 : 0
            };
    }
}
