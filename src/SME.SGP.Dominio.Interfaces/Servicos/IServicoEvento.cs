using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEvento
    {
        Task<string> Salvar(Evento evento, bool alterarRecorrenciaCompleta = false, bool dataConfirmada = false);

        Task SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario);

        void SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada);

        void AlterarRecorrenciaEventos(Evento evento, bool alterarRecorrenciaCompleta);
    }
}