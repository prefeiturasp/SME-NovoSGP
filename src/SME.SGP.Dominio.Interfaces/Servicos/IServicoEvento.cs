using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEvento
    {
        
        void SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario);

        void SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada);
        Task Salvar(Evento evento, bool dataConfirmada = false);
    }
}