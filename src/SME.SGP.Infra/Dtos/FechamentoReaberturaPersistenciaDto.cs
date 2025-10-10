using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FechamentoReaberturaPersistenciaDto
    {
        public int[] Bimestres { get; set; }
        public string Descricao { get; set; }
        public string DreCodigo { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inicio { get; set; }
        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public AplicacaoSondagem Aplicacao { get; set; }
    }
}