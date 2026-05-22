using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class AbrangenciaCompletaRetornoDto
    {
        public IEnumerable<AbrangenciaDreRetornoDto> Dres { get; set; }
        public IEnumerable<AbrangenciaUeIntegracaoRetornoDto> Ues { get; set; }
        public IEnumerable<AbrangenciaTurmaIntegracaoRetornoDto> Turmas { get; set; }
    }

    public class AbrangenciaUeIntegracaoRetornoDto
    {
        public string Codigo { get; set; }
        public string NomeSimples { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool EhInfantil { get; set; }
        public string CodigoDre { get; set; }
    }

    public class AbrangenciaTurmaIntegracaoRetornoDto
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public int CodigoModalidade { get; set; }
        public int Semestre { get; set; }
        public bool EnsinoEspecial { get; set; }
        public long Id { get; set; }
        public int TipoTurma { get; set; }
        public string CodigoUe { get; set; }
    }
}
