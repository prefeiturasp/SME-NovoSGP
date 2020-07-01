using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEvento
    {
        void AlterarRecorrenciaEventos(Evento evento, bool alterarRecorrenciaCompleta);

        Task<string> Salvar(Evento evento, bool alterarRecorrenciaCompleta = false, bool dataConfirmada = false, bool unitOfWorkJaEmUso = false);

        void Excluir(Evento evento);

        void SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario);

        Task SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada);
    }
}