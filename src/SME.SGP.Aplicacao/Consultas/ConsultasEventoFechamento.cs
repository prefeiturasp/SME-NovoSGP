using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasEventoFechamento: IConsultasEventoFechamento
    {
        private readonly IRepositorioEventoFechamento repositorio;
        public ConsultasEventoFechamento(IRepositorioEventoFechamento repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
    }
}
