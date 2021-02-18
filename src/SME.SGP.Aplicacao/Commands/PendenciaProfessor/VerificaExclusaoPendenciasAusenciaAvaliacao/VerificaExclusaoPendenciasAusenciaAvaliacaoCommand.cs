using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaAvaliacaoCommand : IRequest<bool>
    {
        public VerificaExclusaoPendenciasAusenciaAvaliacaoCommand(string turmaCodigo, string[] componentesCurriculares, TipoPendencia tipoPendencia, DateTime dataAvaliacao)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            TipoPendencia = tipoPendencia;
            DataAvaliacao = dataAvaliacao;
        }

        public string TurmaCodigo { get; set; }
        public string[] ComponentesCurriculares { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
        public DateTime DataAvaliacao { get; set; }
    }
}
