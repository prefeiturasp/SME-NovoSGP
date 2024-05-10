﻿using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioListagemOcorrenciasDto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string[] CodigosTurma { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public long[] OcorrenciaTipoIds { get; set; }
        public bool ImprimirDescricaoOcorrencia { get; set; }
        public string NomeUsuario { get; set; }
        public string CodigoRf { get; set; }
    }
}
