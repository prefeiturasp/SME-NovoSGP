using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoNota : IConsultasFechamentoNota
    {
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        public ConsultasFechamentoNota(IRepositorioFechamentoNota repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }
    }
}