using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            fechamentoReabertura.PodeSalvar();
            await VerificaSeTemReaberturasHierarquicas(fechamentoReabertura);
        }

        private bool EstaNoRangeDeDatas(IEnumerable<(DateTime, DateTime)> datas, FechamentoReabertura fechamentoReabertura)
        {
            return datas.Any(a => (fechamentoReabertura.Inicio.Date <= a.Item1.Date && fechamentoReabertura.Fim.Date >= a.Item2.Date)
            || (fechamentoReabertura.Inicio <= a.Item2.Date && fechamentoReabertura.Fim.Date >= a.Item2.Date)
            || (fechamentoReabertura.Inicio >= a.Item1.Date && fechamentoReabertura.Fim.Date <= a.Item2.Date));
        }

        private async Task VerificaSeTemReaberturasHierarquicas(FechamentoReabertura fechamentoReabertura)
        {
            if (fechamentoReabertura.EhParaDre())
            {
                var fechamentoReaberturaSME = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendarioId, null, null);

                if (fechamentoReaberturaSME.Any())
                {
                    var datasDosFechamentosSME = fechamentoReaberturaSME.Select(a => { return (a.Inicio.Date, a.Fim.Date); });

                    if (!EstaNoRangeDeDatas(datasDosFechamentosSME, fechamentoReabertura))
                        throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME neste período informado.");
                }
                else throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME.");
            }
            else if (fechamentoReabertura.EhParaUe())
            {
            }
        }
    }
}