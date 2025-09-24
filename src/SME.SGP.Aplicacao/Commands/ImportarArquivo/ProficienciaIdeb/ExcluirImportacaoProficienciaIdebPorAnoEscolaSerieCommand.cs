using MediatR;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdeb
{
    public class ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommand: IRequest<bool>
    {
        public ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommand(int anoLetivo, string codigoEolEscola, int serieAno)
        {
            AnoLetivo = anoLetivo;
            CodigoEolEscola = codigoEolEscola;
            SerieAno = serieAno;
        }
        public int AnoLetivo { get; }
        public string CodigoEolEscola { get; }
        public int SerieAno { get; }
    }
}
