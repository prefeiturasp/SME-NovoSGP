using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoAulaTeste
    {
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly IServicoAula servicoAula;
        private readonly Mock<IServicoDiaLetivo> servicoDiaLetivo;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly Mock<IServicoLog> servicoLog;

        public ServicoAulaTeste()
        {
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();

            servicoDiaLetivo = new Mock<IServicoDiaLetivo>();
            repositorioAula = new Mock<IRepositorioAula>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            servicoLog = new Mock<IServicoLog>();
            servicoEol = new Mock<IServicoEOL>();
            servicoAula = new ServicoAula(repositorioAula.Object, servicoEol.Object, repositorioTipoCalendario.Object, servicoDiaLetivo.Object, repositorioPeriodoEscolar.Object, servicoLog.Object);
        }

        [Fact]
        public async void Deve_Incluir_Aula()
        {
            //ARRANGE
            var aula = new Aula() { DisciplinaId = "1", UeId = "1", DataAula = DateTime.Now, RecorrenciaAula = RecorrenciaAula.AulaUnica };
            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D") } });
            var tipoCalendario = new TipoCalendario();
            IEnumerable<DisciplinaResposta> disciplinaRespotas = new List<DisciplinaResposta>() { new DisciplinaResposta() { CodigoComponenteCurricular = 1 } };

            repositorioAula.Setup(a => a.UsuarioPodeCriarAulaNaUeTurmaEModalidade(aula, tipoCalendario.Modalidade)).Returns(true);

            repositorioTipoCalendario.Setup(a => a.ObterPorId(It.IsAny<long>())).Returns(tipoCalendario);

            servicoEol.Setup(a => a.ObterDisciplinasPorCodigoTurmaLoginEPerfil(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(disciplinaRespotas));

            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(new PeriodoEscolar());

            servicoDiaLetivo.Setup(a => a.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId)).Returns(true);

            //ACT
            await servicoAula.Salvar(aula, usuario);

            //ASSERT
            repositorioAula.Verify(c => c.Salvar(aula), Times.Once);
        }

        [Fact]
        public async void Deve_Incluir_Aula_Recorrencia()
        {
            //ARRANGE
            var aula = new Aula() { DisciplinaId = "1", UeId = "1", DataAula = new DateTime(2019, 1, 1), RecorrenciaAula = RecorrenciaAula.RepetirBimestreAtual };
            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D") } });
            var tipoCalendario = new TipoCalendario();
            IEnumerable<DisciplinaResposta> disciplinaRespotas = new List<DisciplinaResposta>() { new DisciplinaResposta() { CodigoComponenteCurricular = 1 } };
            var periodoEscolar = new PeriodoEscolar() { PeriodoInicio = new DateTime(2019, 1, 1), PeriodoFim = new DateTime(2019, 1, 31) };

            repositorioAula.Setup(a => a.UsuarioPodeCriarAulaNaUeTurmaEModalidade(aula, tipoCalendario.Modalidade)).Returns(true);

            repositorioTipoCalendario.Setup(a => a.ObterPorId(It.IsAny<long>())).Returns(tipoCalendario);

            servicoEol.Setup(a => a.ObterDisciplinasPorCodigoTurmaLoginEPerfil(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(disciplinaRespotas));

            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(periodoEscolar);
            repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(aula.TipoCalendarioId)).Returns(new List<PeriodoEscolar>() { periodoEscolar });

            servicoDiaLetivo.Setup(a => a.ValidarSeEhDiaLetivo(aula.DataAula, aula.TipoCalendarioId, null, aula.UeId)).Returns(true);

            //ACT
            await servicoAula.Salvar(aula, usuario);

            //ASSERT
            repositorioAula.Verify(c => c.Salvar(aula), Times.Exactly(1));
        }
    }
}