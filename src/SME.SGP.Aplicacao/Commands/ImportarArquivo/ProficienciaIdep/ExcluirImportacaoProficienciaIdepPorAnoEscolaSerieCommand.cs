using MediatR;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand: IRequest<bool>
    {
        public ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand(int anoLetivo, string codigoEolEscola, int serieAno)
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
