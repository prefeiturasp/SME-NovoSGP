﻿using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioPendenciasDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string[] TurmasCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public int? Bimestre { get; set; }
        public int? Semestre { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
        public bool ExibirPendenciasResolvidas { get; set; }
        public int[] TipoPendenciaGrupo { get; set; }
        public bool ExibirHistorico { get; set; }
        public string UsuarioLogadoNome { get; set; }
        public string UsuarioLogadoRf { get; set; }

    }
}