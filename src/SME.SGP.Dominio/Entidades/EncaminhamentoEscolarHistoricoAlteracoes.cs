using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    [ExcludeFromCodeCoverage]
    public class EncaminhamentoEscolarHistoricoAlteracoes
    {
        public long Id { get; set; }
        public long EncaminhamentoEscolarId { get; set; }
        public long? SecaoEncaminhamentoEscolarId { get; set; }
        public long UsuarioId { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoEscolarSecao { get; set; }
        public Usuario Usuario { get; set; }
        public string CamposInseridos { get; set; }
        public string CamposAlterados { get; set; }
        public string DataAtendimento { get; set; }
        public DateTime DataHistorico { get; set; }
        public TipoHistoricoAlteracoesAtendimentoNAAPA TipoHistorico { get; set; }
    }
}