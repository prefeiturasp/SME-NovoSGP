using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanoAEEReduzidoDto
    {
        public long Id { get; set; }
        public string EstudanteNome { get; set; }
        public string EstudanteCodigo { get; set; }
        public string UENome { get; set; }
        public string UECodigo { get; set; }
        public string DRECodigo { get; set; }
        public string DREAbreviacao { get; set; }
        public TipoEscola UETipo { get; set; }
        public DateTime VigenciaInicio { get; set; }
        public DateTime VigenciaFim { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public string TurmaNome { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
    }

}
