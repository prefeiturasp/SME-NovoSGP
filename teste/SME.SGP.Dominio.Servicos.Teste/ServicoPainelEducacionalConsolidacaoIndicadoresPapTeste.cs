using FluentAssertions;
using SME.SGP.Dominio.Servicos;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste.Servicos
{
    public class ServicoPainelEducacionalConsolidacaoIndicadoresPapTeste
    {
        private readonly ServicoPainelEducacionalConsolidacaoIndicadoresPap _servico;

        public ServicoPainelEducacionalConsolidacaoIndicadoresPapTeste()
        {
            _servico = new ServicoPainelEducacionalConsolidacaoIndicadoresPap();
        }

        [Fact]
        public void ConsolidarDados_ComCenarioComplexo_DeveGerarVisoesConsistentesEAgregadasCorretamente()
        {
            // Arrange
            var dadosAlunos = ObterDadosMatriculaAlunosCenarioComplexo();
            var dadosFrequencia = ObterDadosFrequenciaCenarioComplexo();
            var dadosIndicadores = ObterDadosIndicadoresCenarioComplexo();

            // Act
            var (sme, dre, ue) = _servico.ConsolidarDados(dadosAlunos, dadosIndicadores, dadosFrequencia);

            // Assert

            // 1. Validação da Estrutura Geral
            ue.Should().HaveCount(3);
            dre.Should().HaveCount(2);
            sme.Should().HaveCount(1);

            // 2. Validação de Consistência: Soma(UEs) por DRE == Total da DRE
            var dre1 = dre.First(d => d.CodigoDre == "DRE-1");
            var uesDaDre1 = ue.Where(u => u.CodigoDre == "DRE-1");

            uesDaDre1.Sum(u => u.TotalTurmas).Should().Be(dre1.TotalTurmas);
            uesDaDre1.Sum(u => u.TotalAlunos).Should().Be(dre1.TotalAlunos);
            uesDaDre1.Sum(u => u.TotalAlunosComFrequenciaInferiorLimite).Should().Be(dre1.TotalAlunosComFrequenciaInferiorLimite);

            // 3. Validação de Consistência: Soma(DREs) por Ano/TipoPap == Total da SME
            var smeConsolidado = sme.First();
            dre.Sum(d => d.TotalTurmas).Should().Be(smeConsolidado.TotalTurmas);
            dre.Sum(d => d.TotalAlunos).Should().Be(smeConsolidado.TotalAlunos);
            dre.Sum(d => d.TotalAlunosComFrequenciaInferiorLimite).Should().Be(smeConsolidado.TotalAlunosComFrequenciaInferiorLimite);

            // 4. Validação Detalhada de um caso (UE-11 na DRE-1)
            var ue11 = ue.First(u => u.CodigoUe == "UE-11");
            ue11.TotalTurmas.Should().Be(2); // Turma 1 e 2
            ue11.TotalAlunos.Should().Be(3); // Aluno 101, 102, 103
            ue11.TotalAlunosComFrequenciaInferiorLimite.Should().Be(15); // Turma 1 (10) + Turma 2 (5)
            ue11.NomeDificuldadeTop1.Should().Be("Leitura");
            ue11.TotalAlunosDificuldadeTop1.Should().Be(50);
            ue11.NomeDificuldadeTop2.Should().Be("Escrita");
            ue11.TotalAlunosDificuldadeTop2.Should().Be(25);
            ue11.TotalAlunosDificuldadeOutras.Should().Be(5);
        }

        [Fact]
        public void ConsolidarDados_QuandoNaoExistemDadosDeEntrada_DeveRetornarListasVazias()
        {
            // Arrange
            var dadosAlunosVazio = new List<DadosMatriculaAlunoTipoPapDto>();
            var dadosFrequenciaVazio = new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>();
            var dadosIndicadoresVazio = new List<ContagemDificuldadeIndicadoresPapPorTipoDto>();

            // Act
            var (sme, dre, ue) = _servico.ConsolidarDados(dadosAlunosVazio, dadosIndicadoresVazio, dadosFrequenciaVazio);

            // Assert
            sme.Should().BeEmpty();
            dre.Should().BeEmpty();
            ue.Should().BeEmpty();
        }

        [Fact]
        public void ConsolidarDados_QuandoAlunosExistemMasFrequenciaNao_DeveCalcularFrequenciaComoZero()
        {
            // Arrange
            var dadosAlunos = new List<DadosMatriculaAlunoTipoPapDto>
            {
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = 2025, CodigoDre = "DRE-X", CodigoUe = "UE-Y", TipoPap = TipoPap.PapColaborativo, CodigoTurma = 99, CodigoAluno = 999 }
            };
            var dadosFrequenciaVazio = new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>();
            var dadosIndicadoresVazio = new List<ContagemDificuldadeIndicadoresPapPorTipoDto>();

            // Act
            var (sme, dre, ue) = _servico.ConsolidarDados(dadosAlunos, dadosIndicadoresVazio, dadosFrequenciaVazio);

            // Assert
            ue.First().TotalAlunosComFrequenciaInferiorLimite.Should().Be(0);
            dre.First().TotalAlunosComFrequenciaInferiorLimite.Should().Be(0);
            sme.First().TotalAlunosComFrequenciaInferiorLimite.Should().Be(0);
        }

        [Fact]
        public void ConsolidarDados_QuandoNaoExistemIndicadores_DeveRetornarDificuldadesVazias()
        {
            // Arrange
            var dadosAlunos = new List<DadosMatriculaAlunoTipoPapDto>
            {
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = 2025, CodigoDre = "DRE-X", CodigoUe = "UE-Y", TipoPap = TipoPap.PapColaborativo, CodigoTurma = 99, CodigoAluno = 999 }
            };
            var dadosFrequencia = new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>();
            var dadosIndicadoresVazio = new List<ContagemDificuldadeIndicadoresPapPorTipoDto>();

            // Act
            var (sme, dre, ue) = _servico.ConsolidarDados(dadosAlunos, dadosIndicadoresVazio, dadosFrequencia);

            // Assert
            var ueConsolidado = ue.First();
            ueConsolidado.NomeDificuldadeTop1.Should().BeEmpty();
            ueConsolidado.TotalAlunosDificuldadeTop1.Should().Be(0);
            ueConsolidado.NomeDificuldadeTop2.Should().BeEmpty();
            ueConsolidado.TotalAlunosDificuldadeTop2.Should().Be(0);
            ueConsolidado.TotalAlunosDificuldadeOutras.Should().Be(0);
        }

        #region Geração de Massa de Teste
        private static IEnumerable<DadosMatriculaAlunoTipoPapDto> ObterDadosMatriculaAlunosCenarioComplexo()
        {
            const int ano = 2025;
            const TipoPap tipoPap = TipoPap.PapColaborativo;

            return new List<DadosMatriculaAlunoTipoPapDto>
            {
                // DRE-1 / UE-11: 2 turmas, 3 alunos
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", CodigoTurma = 1, CodigoAluno = 101, TipoPap = tipoPap },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", CodigoTurma = 1, CodigoAluno = 102, TipoPap = tipoPap },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", CodigoTurma = 2, CodigoAluno = 103, TipoPap = tipoPap },
                // DRE-1 / UE-12: 1 turma, 2 alunos (aluno 201 é novo, aluno 101 já existe na UE-11, mas conta para esta UE também)
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-12", CodigoTurma = 3, CodigoAluno = 101, TipoPap = tipoPap },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-12", CodigoTurma = 3, CodigoAluno = 201, TipoPap = tipoPap },
                // DRE-2 / UE-21: 1 turma, 2 alunos
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-2", CodigoUe = "UE-21", CodigoTurma = 4, CodigoAluno = 301, TipoPap = tipoPap },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = ano, CodigoDre = "DRE-2", CodigoUe = "UE-21", CodigoTurma = 4, CodigoAluno = 302, TipoPap = tipoPap },
            };
        }

        private static IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> ObterDadosFrequenciaCenarioComplexo()
        {
            return new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>
            {
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 1, QuantidadeAbaixoMinimoFrequencia = 10 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 2, QuantidadeAbaixoMinimoFrequencia = 5 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 3, QuantidadeAbaixoMinimoFrequencia = 20 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 4, QuantidadeAbaixoMinimoFrequencia = 30 },
            };
        }

        private static IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> ObterDadosIndicadoresCenarioComplexo()
        {
            const int ano = 2025;
            const TipoPap tipoPap = TipoPap.PapColaborativo;

            return new List<ContagemDificuldadeIndicadoresPapPorTipoDto>
            {
                // Indicadores para UE-11
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", TipoPap = tipoPap, RespostaId = 1, NomeDificuldade = "Leitura", Quantidade = 50 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", TipoPap = tipoPap, RespostaId = 2, NomeDificuldade = "Escrita", Quantidade = 25 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = "UE-11", TipoPap = tipoPap, RespostaId = PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP, NomeDificuldade = PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP, Quantidade = 5 },
                
                // Indicadores apenas no nível DRE-1 (deve ser encontrado ao consolidar DRE-1)
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = null, TipoPap = tipoPap, RespostaId = 3, NomeDificuldade = "Cálculo", Quantidade = 100 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = "DRE-1", CodigoUe = null, TipoPap = tipoPap, RespostaId = 4, NomeDificuldade = "Raciocínio Lógico", Quantidade = 80 },

                // Indicadores apenas no nível SME (deve ser encontrado ao consolidar SME)
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = null, CodigoUe = null, TipoPap = tipoPap, RespostaId = 5, NomeDificuldade = "Interpretação", Quantidade = 200 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = ano, CodigoDre = null, CodigoUe = null, TipoPap = tipoPap, RespostaId = 6, NomeDificuldade = "Produção textual", Quantidade = 150 },
            };
        }
        #endregion
    }
}