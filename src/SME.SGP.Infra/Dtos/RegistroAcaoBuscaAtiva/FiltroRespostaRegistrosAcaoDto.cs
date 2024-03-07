using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroRespostaRegistrosAcaoDto
    {
        public string CodigoNomeAluno { get; set; }
        public DateTime? DataRegistroInicio { get; set; }
        public DateTime? DataRegistroFim { get; set; }
        public int? OrdemRespostaQuestaoProcedimentoRealizado { get; set; }
    }
}