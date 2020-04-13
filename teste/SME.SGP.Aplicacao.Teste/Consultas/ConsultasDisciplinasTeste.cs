using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasDisciplinasTeste
    {
        private readonly ConsultasDisciplina consultasDisciplinas;
        private readonly Mock<IConsultasObjetivoAprendizagem> consultasObjetivoAprendizagem;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IRepositorioComponenteCurricular> repositorioComponenteCurricular;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ConsultasDisciplinasTeste()
        {
            servicoEol = new Mock<IServicoEOL>();
            repositorioCache = new Mock<IRepositorioCache>();
            consultasObjetivoAprendizagem = new Mock<IConsultasObjetivoAprendizagem>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioComponenteCurricular = new Mock<IRepositorioComponenteCurricular>();
            consultasDisciplinas = new ConsultasDisciplina(servicoEol.Object,
                                                           repositorioCache.Object,
                                                           consultasObjetivoAprendizagem.Object,
                                                           servicoUsuario.Object,
                                                           repositorioAtribuicaoCJ.Object,
                                                           repositorioComponenteCurricular.Object);
        }

        [Fact(DisplayName = "DeveObterDisciplinasParaPlanejamento")]
        public async Task DeveObterDisciplinasParaPlanejamento()
        {
            var disciplinas = new List<DisciplinaResposta>
            {
                new DisciplinaResposta
                {
                    CodigoComponenteCurricular=1,
                    Nome="regencia",
                    Regencia=true
                },
                new DisciplinaResposta
                {
                    CodigoComponenteCurricular=1,
                    Nome="nao regencia",
                    Regencia=false
                }
            };
            servicoEol.Setup(c => c.ObterDisciplinasParaPlanejamento(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<Guid>()))
                  .Returns(Task.FromResult<IEnumerable<DisciplinaResposta>>(disciplinas));

            servicoUsuario.Setup(c => c.ObterRf())
                .Returns("123");

            var retorno = await consultasDisciplinas.ObterComponentesCurricularesPorProfessorETurmaParaPlanejamento(0, "10", false, false);
            Assert.True(retorno != null);
            Assert.True(retorno.Any());
            Assert.Contains(retorno, c => c.Regencia);
            Assert.Contains(retorno, c => !c.Regencia);
        }
    }
}