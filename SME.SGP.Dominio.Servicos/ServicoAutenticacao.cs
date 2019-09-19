using SME.SGP.Aplicacao.Integracoes;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAutenticacao
    {
        private readonly IServicoEOL servicoEOL;

        public ServicoAutenticacao(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public void AutenticarNoEol(string login, string senha)
        {
        }
    }
}