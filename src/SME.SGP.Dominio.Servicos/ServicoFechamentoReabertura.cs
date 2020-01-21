using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamentoReabertura : IServicoFechamentoReabertura
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ServicoFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task Salvar(FechamentoReabertura fechamentoReabertura)
        {
            var fechamentoReaberturas = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

            fechamentoReabertura.PodeSalvar(fechamentoReaberturas);
            //await VerificaSeTemReaberturasHierarquicas(fechamentoReabertura);
        }

        //private async Task VerificaSeTemReaberturasHierarquicas(FechamentoReabertura fechamentoReabertura)
        //{
        //    if (fechamentoReabertura.EhParaDre())
        //    {
        //        var fechamentoReaberturaSME = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

        //        if (fechamentoReaberturaSME.Any())
        //        {
        //            var datasDosFechamentosSME = fechamentoReaberturaSME.Select(a => { return (a.Inicio.Date, a.Fim.Date); });
        //            fechamentoReabertura.PodePersistirNesteNasDatas(datasDosFechamentosSME);
        //        }
        //        else throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME.");
        //    }
        //    else if (fechamentoReabertura.EhParaUe())
        //    {
        //        var fechamentoReaberturaDre = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, fechamentoReabertura.DreId, null);

        //        if (fechamentoReaberturaDre.Any())
        //        {
        //            var datasDosFechamentosSME = fechamentoReaberturaDre.Select(a => { return (a.Inicio.Date, a.Fim.Date); });
        //            fechamentoReabertura.PodePersistirNesteNasDatas(datasDosFechamentosSME);
        //        }
        //        else
        //        {
        //            var fechamentoReaberturaSME = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

        //            if (fechamentoReaberturaSME.Any())
        //            {
        //                var datasDosFechamentosSME = fechamentoReaberturaSME.Select(a => { return (a.Inicio.Date, a.Fim.Date); });
        //                fechamentoReabertura.PodePersistirNesteNasDatas(datasDosFechamentosSME);
        //            }
        //            else throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME.");
        //        }
        //    }
        //}
    }
}