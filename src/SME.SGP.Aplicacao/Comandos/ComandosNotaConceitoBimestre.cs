using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotaConceitoBimestre : IComandosNotaConceitoBimestre
    {
        private readonly IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre;

        public ComandosNotaConceitoBimestre(IRepositorioNotaConceitoBimestre repositorioNotaConceitoBimestre)
        {
            this.repositorioNotaConceitoBimestre = repositorioNotaConceitoBimestre ?? throw new ArgumentNullException(nameof(repositorioNotaConceitoBimestre));
        }


    }
}