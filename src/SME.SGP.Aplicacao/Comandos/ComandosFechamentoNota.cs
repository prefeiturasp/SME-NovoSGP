using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoNota : IComandosFechamentoNota
    {
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;

        public ComandosFechamentoNota(IRepositorioFechamentoNota repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }


    }
}