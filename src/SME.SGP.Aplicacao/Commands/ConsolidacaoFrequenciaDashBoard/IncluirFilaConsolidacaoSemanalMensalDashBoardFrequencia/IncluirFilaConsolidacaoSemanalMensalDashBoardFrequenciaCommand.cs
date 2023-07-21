using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(long turmaId, string codigoTurma, bool ehModalidadeInfantil, int anoLetivo, DateTime dataAula)
        {
            TurmaId = turmaId;
            DataAula = dataAula;
            EhModalidadeInfantil = ehModalidadeInfantil;
            CodigoTurma = codigoTurma;
            AnoLetivo = anoLetivo;
        }
        
        public long TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public bool EhModalidadeInfantil { get; set; }
        public string CodigoTurma { get; set; }
        public int AnoLetivo { get; set; }
    }
}
