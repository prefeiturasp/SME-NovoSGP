using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoFormatoRelatorio
    {
        [Display(Name = "pdf")]
        Pdf = 1,
        [Display(Name = "html")]
        Html = 2,
        [Display(Name = "xls")]
        Xls = 3,
        [Display(Name = "xlsx")]
        Xlsx = 4,
        [Display(Name = "rtf")]
        Rtf = 5,
        [Display(Name = "csv")]
        Csv = 6,
        [Display(Name = "xml")]
        Xml = 7,
        [Display(Name = "docx")]
        Docx = 8,
        [Display(Name = "odt")]
        Odt = 9,
        [Display(Name = "ods")]
        Ods = 10,
        [Display(Name = "jrprint")]
        Jrprint = 11,
    }
}
