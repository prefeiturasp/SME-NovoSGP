using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoAnual : IConsultasPlanoAnual
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;

        public ConsultasPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                   IConsultasTipoCalendario consultasTipoCalendario,
                                   IConsultasPeriodoEscolar consultasPeriodoEscolar)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnual));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(consultasPeriodoEscolar));
        }

        public async Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            var planoAnual = repositorioPlanoAnual.ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
            if (planoAnual != null)
            {
                var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();

                if (planoAnual.IdsObjetivosAprendizagem == null)
                    return planoAnual;

                planoAnual.EhExpandido = VerificaSePeriodoEhExpandido(planoAnual.AnoLetivo.Value, Modalidade.Fundamental, planoAnual.Bimestre);

                foreach (var idObjetivo in planoAnual.IdsObjetivosAprendizagem)
                {
                    var objetivo = objetivosAprendizagem.FirstOrDefault(c => c.Id == idObjetivo);
                    if (objetivo != null)
                    {
                        planoAnual.ObjetivosAprendizagem.Add(objetivo);
                    }
                }
            }
            return planoAnual;
        }

        public bool VerificaSePeriodoEhExpandido(int anoLetivo, Modalidade modalidade, int bimestre)
        {
            var tipoCalendario = consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);

            if (tipoCalendario == null)
                return false;

            var periodoEscolar = consultasPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodoEscolar == null)
                return false;

            var periodo = periodoEscolar.Periodos.FirstOrDefault(p => p.Bimestre == bimestre);

            if (periodo == null)
                return false;

            var dataAtual = DateTime.Now;

            return periodo.PeriodoInicio <= dataAtual || periodo.PeriodoFim >= dataAtual;
        }

        public bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            return repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
        }
    }
}