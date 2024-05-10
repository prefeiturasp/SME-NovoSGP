using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using System;
using System.Linq;

namespace SME.SGP.Infra
{
    public class TurmaComplementarDto
    {
        public static readonly TipoTurma[] TiposRegulares = { TipoTurma.Regular, TipoTurma.EdFisica, TipoTurma.Itinerarios2AAno };
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoTurma { get; set; }
        public TipoTurma TipoTurma { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long Id { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public string Nome { get; set; }
        public int Semestre { get; set; }
        public int TipoTurno { get; set; }
        public string SerieEnsino { get; set; }
        public string NomeFiltro { get; set; }
        public bool Historica { get; set; }
        public bool EhTurmaFund1 => (ModalidadeCodigo == Modalidade.Fundamental && AnoTurmaInteiro >= 1 && AnoTurmaInteiro <= 5);
        public bool EhTurmaFund2 => (ModalidadeCodigo == Modalidade.Fundamental && AnoTurmaInteiro >= 6 && AnoTurmaInteiro <= 9);
        public bool EhTurmaEnsinoMedio => ModalidadeCodigo == Modalidade.Medio;
        public bool EhTurmaInfantil => ModalidadeCodigo == Modalidade.EducacaoInfantil;
        public bool EhTurmaHistorica => AnoLetivo < DateTime.Now.Year;
        public bool EhEJA()
            => ModalidadeCodigo == Modalidade.EJA;
        public int AnoTurmaInteiro => Ano.ToCharArray().All(a => char.IsDigit(a)) ? int.Parse(Ano) : 1;
        public string TurmaRegularCodigo { get; set; }

    }
}
