using MediatR;
using Moq;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandlerTeste
    {
        private readonly ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandler query;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> repositorioFrequenciaAlunoDisciplinaPeriodoConsulta;
        public ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandlerTeste()
        {
            repositorioFrequenciaAlunoDisciplinaPeriodoConsulta = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            query = new ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandler(repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.Object);
        }

        [Fact(DisplayName = "ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQueryHandler -  Obter percentual de frequência vazio caso não tenha freq. consolidada")]
        public async Task Deve_obter_percentual_de_frequencia_formatado_nulo_com_nenhuma_frequencia_registrada()
        {
            var frequenciasAlunos = new List<FrequenciaAluno>()
            {
                new FrequenciaAluno
                {
                    Bimestre = 1,
                    CodigoAluno = "1",
                    TurmaId = "1",
                    DisciplinaId = "1",
                    TotalAulas = 0,
                    TotalAusencias = 0,
                    TotalCompensacoes = 0,
                    TotalPresencas = 0,
                    TotalRemotos = 0,
                }
            };

            IEnumerable<FrequenciaAluno> frequencias = frequenciasAlunos;

            repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.Setup(x => x.ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<string[]>()))
               .ReturnsAsync(frequencias);

            var retornoConsulta = await query.Handle(new ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaQuery("1", new string[] { "1" }, new string[] { "1" }), new CancellationToken());

            Assert.NotNull(retornoConsulta);
            Assert.True(retornoConsulta.Count() == 1);
            Assert.True(String.IsNullOrEmpty(retornoConsulta.FirstOrDefault().PercentualFrequenciaFormatado));
        }
    }
}
