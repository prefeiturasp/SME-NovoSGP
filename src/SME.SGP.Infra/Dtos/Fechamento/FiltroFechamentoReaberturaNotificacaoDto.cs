using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroFechamentoReaberturaNotificacaoDto
    {
        public FiltroFechamentoReaberturaNotificacaoDto(string dreCodigo, string ueCodigo, long id, string codigoRf, string tipoCalendarioNome, string ueNome, string dreAbreviacao, 
                                                        DateTime inicio, DateTime fim, string bimestreNome, bool ehParaUe, int anoLetivo, int[] modalidades)
        {
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Id = id;
            CodigoRf = codigoRf;
            TipoCalendarioNome = tipoCalendarioNome;
            UeNome = ueNome;
            DreAbreviacao = dreAbreviacao;
            Inicio = inicio;
            Fim = fim;
            Bimestres = bimestreNome;
            EhParaUe = ehParaUe;
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
        }

        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public long Id { get; set; }
        public string CodigoRf { get; set; }
        public string TipoCalendarioNome { get; set; }
        public string UeNome { get; set; }
        public string DreAbreviacao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public string Bimestres { get; set; }
        public bool EhParaUe { get; set; }
        public int AnoLetivo { get; set; }
        public int[] Modalidades { get; set; }
    }
}