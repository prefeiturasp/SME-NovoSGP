using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosGrade: IComandosGrade
    {
        private readonly IRepositorioGrade repositorioGrade;

        public ComandosGrade(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
        }
    }
}
