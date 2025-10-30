using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepCommand: IRequest<bool>
    {
        public ExcluirImportacaoProficienciaIdepCommand(int anoLetivo, string codigoUe, SerieAnoIndiceDesenvolvimentoEnum serieAno, ComponenteCurricularEnum componenteCurricular)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            SerieAno = serieAno;
            ComponenteCurricular = componenteCurricular;
        }
        public int AnoLetivo { get; }
        public string CodigoUe { get; }
        public SerieAnoIndiceDesenvolvimentoEnum SerieAno { get; }
        public ComponenteCurricularEnum ComponenteCurricular { get; }
    }
}
