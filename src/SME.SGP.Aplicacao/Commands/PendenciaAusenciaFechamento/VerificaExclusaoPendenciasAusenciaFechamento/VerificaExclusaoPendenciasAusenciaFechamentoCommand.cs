using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasAusenciaFechamentoCommand : IRequest<bool>
    {
        
        public VerificaExclusaoPendenciasAusenciaFechamentoCommand(long disciplinaId, int bimestre, long fechamentoId, string turmaCodigo, string codigoRf, TipoPendencia ausenciaFechamento)
        {
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
            FechamentoId = fechamentoId;
            TurmaCodigo = turmaCodigo;
            CodigoRf = codigoRf;
            TipoPendencia = ausenciaFechamento;
        }
        public long DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public long FechamentoId { get; set; }
        public string TurmaCodigo { get; set; }
        public string CodigoRf { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }
}
