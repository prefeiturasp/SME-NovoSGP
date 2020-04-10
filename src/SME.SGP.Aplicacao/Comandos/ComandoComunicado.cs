using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ComandoComunicado : IComandoComunicado
    {
        private readonly IRepositorioComunicado repositorio;

        public ComandoComunicado(IRepositorioComunicado repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }
    }
}