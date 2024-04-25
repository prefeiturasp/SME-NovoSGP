using Moq;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoAlunoTeste
    {
        private readonly ServicoAluno servicoAluno;

        public ServicoAlunoTeste()
        {
            servicoAluno = new ServicoAluno();
        }

        [Fact(DisplayName = "ServicoAluno - Obter marcador do aluno com situação transferido SED")]
        public void ObterMarcadorAlunoComSituacaoTransferidoSed()
        {
            var aluno = new AlunoPorTurmaResposta()
            {
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.TransferidoSED,
                DataSituacao = DateTimeExtension.HorarioBrasilia().AddDays(-10)
            };

            var resultado = servicoAluno.ObterMarcadorAluno(aluno, It.IsAny<PeriodoEscolar>());

            Assert.NotNull(resultado);
            Assert.Equal(TipoMarcadorFrequencia.Transferido, resultado.Tipo);
            Assert.Equal($"{MensagemNegocioAluno.ESTUDANTE_TRANSFERIDO}: {MensagemNegocioAluno.PARA_OUTRAS_REDES} {MensagemNegocioAluno.EM} {aluno.DataSituacao:dd/MM/yyyy}", resultado.Descricao);
        }
    }
}
