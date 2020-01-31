using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ComandosEventoFechamento: IComandosEventoFechamento
    {
        private readonly IRepositorioEventoFechamento repositorio;

        public ComandosEventoFechamento(IRepositorioEventoFechamento repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
    }
}
