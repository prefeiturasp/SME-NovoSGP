using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ConsultaComunicado : ConsultasBase, IConsultaComunicado
    {
        private readonly IRepositorioComunicado repositorio;

        public ConsultaComunicado(
            IRepositorioComunicado repositorio,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
    }
}