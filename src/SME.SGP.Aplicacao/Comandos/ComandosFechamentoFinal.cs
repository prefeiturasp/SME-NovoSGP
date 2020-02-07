using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IServicoFechamentoFinal fechamentoFinal;

        public ComandosFechamentoFinal(IServicoFechamentoFinal fechamentoFinal)
        {
            this.fechamentoFinal = fechamentoFinal ?? throw new System.ArgumentNullException(nameof(fechamentoFinal));
        }

        public async Task SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var fechamentoFinal = TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto);
        }

        private static FechamentoFinal TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var fechamentoFinal = new FechamentoFinal();
            if (fechamentoFinalSalvarDto.EhNota())
                fechamentoFinal.Nota = fechamentoFinalSalvarDto.Nota;
            else
            {
                fechamentoFinal.Conceito
            }

            return fechamentoFinal;
        }
    }
}