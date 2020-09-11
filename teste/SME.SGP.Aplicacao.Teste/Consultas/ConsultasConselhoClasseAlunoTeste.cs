
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasConselhoClasseAlunoTeste
    {
        private readonly ConsultasConselhoClasseAluno consultasConselhoClasseAluno;
        private readonly Mock<IConsultasAulaPrevista> consultasAulaPrevista;
        private readonly Mock<IConsultasConselhoClasseNota> consultasConselhoClasseNota;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasFechamentoNota> consultasFechamentoNota;
        private readonly Mock<IConsultasFechamentoTurma> consultasFechamentoTurma;
        private readonly Mock<IConsultasFrequencia> consultasFrequencia;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasTipoCalendario> consultasTipoCalendario;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioConselhoClasseAluno> repositorioConselhoClasseAluno;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> repositorioFrequenciaAluno;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly Mock<IServicoConselhoClasse> servicoConselhoClasse;
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly Mock<IServicoUsuario> servicoUsuario;
    }
}
