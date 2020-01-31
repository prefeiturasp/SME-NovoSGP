using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoRecuperacaoParalela : IServicoRecuperacaoParalela
    {
        public ServicoRecuperacaoParalela()
        {
        }

        public RecuperacaoParalelaStatus ObterStatusRecuperacaoParalela(int RespostasRecuperacaoParalela, int Objetivos)
        {
            if (RespostasRecuperacaoParalela == Objetivos) return RecuperacaoParalelaStatus.Concluido;
            if (RespostasRecuperacaoParalela > 0) return RecuperacaoParalelaStatus.Alerta;
            return RecuperacaoParalelaStatus.NaoAlterado;
        }
    }
}