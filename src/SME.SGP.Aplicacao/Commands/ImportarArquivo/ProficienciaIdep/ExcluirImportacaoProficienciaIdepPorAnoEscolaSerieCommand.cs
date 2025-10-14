using MediatR;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand: IRequest<bool>
    {
        public ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(int anoLetivo, string codigoEolEscola, int serieAno, string componenteCurricular)
        {
            AnoLetivo = anoLetivo;
            CodigoEolEscola = codigoEolEscola;
            SerieAno = serieAno;
            ComponenteCurricular = componenteCurricular;
        }
        public int AnoLetivo { get; }
        public string CodigoEolEscola { get; }
        public int SerieAno { get; }
        public string ComponenteCurricular { get; }
    }
}
