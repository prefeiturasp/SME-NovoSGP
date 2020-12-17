using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioLeituraComunicados
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public long AnoLetivo { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public string Turma { get; set; }
        public List<long> Grupos { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string NotificacaoId { get; set; }
        public bool ListarResponsaveisEstudantes { get; set; }
        public bool ListarComunicadosExpirados { get; set; }
        public string NomeUsuario { get; set; }
    }
}
