using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiarioBordoDetalhesParaPendenciaDto
    {
        public long Id { get; set; }
        public string NomeEscola { get; set; }
        public string DescricaoComponenteCurricular { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long TurmaId { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public string NomeTurma { get; set; }
        public string CodigoTurma { get; set; }
        public int CodModalidadeTurma { get; set; }
        public int PeriodoEscolarId { get; set; }
        public string RetornarTurmaComModalidade()
        {
            var modalidadeEnum = ((Modalidade)CodModalidadeTurma);
            return string.Format("{0}-{1}", modalidadeEnum.ShortName(), NomeTurma);
        }
    }
}
