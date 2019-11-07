using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoAnual : IConsultasPlanoAnual
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ConsultasPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                   IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                   IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnual));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<PlanoAnualCompletoDto> ObterBimestreExpandido(FiltroPlanoAnualBimestreExpandidoDto filtro)
        {
            var planoAnualLista = new List<PlanoAnualCompletoDto>();

            var bimestres = filtro.ModalidadePlanoAnual == Modalidade.EJA ? 2 : 4;

            var filtroPlanoAnualDto = ObtenhaFiltro(filtro.AnoLetivo, filtro.ComponenteCurricularEolId, filtro.EscolaId, filtro.TurmaId, 0);

            for (int i = 1; i <= bimestres; i++)
            {
                filtroPlanoAnualDto.Bimestre = i;

                planoAnualLista.Add(await ObterPorEscolaTurmaAnoEBimestre(filtroPlanoAnualDto));
            }

            var periodosEscolares = ObterPeriodoEscolar(filtro.AnoLetivo, filtro.ModalidadePlanoAnual);

            if (periodosEscolares == null)
                return null;

            var retorno = planoAnualLista.FirstOrDefault(x => VerificaSeBimestreEhExpandido(periodosEscolares, x.Bimestre));

            return retorno;
        }

        public async Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            var planoAnual = repositorioPlanoAnual.ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
            if (planoAnual != null)
            {
                var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();

                if (planoAnual.IdsObjetivosAprendizagem == null)
                    return planoAnual;

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

        public bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            return repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }

        private FiltroPlanoAnualDto ObtenhaFiltro(int anoLetivo, long componenteCurricularEolId, string escolaId, int turmaId, int bimestre)
        {
            return new FiltroPlanoAnualDto
            {
                AnoLetivo = anoLetivo,
                ComponenteCurricularEolId = componenteCurricularEolId,
                EscolaId = escolaId,
                TurmaId = turmaId,
                Bimestre = bimestre
            };
        }

        private IEnumerable<PeriodoEscolar> ObterPeriodoEscolar(int anoLetivo, Modalidade modalidade)
        {
            var modalidadeTipoCalendario = ModalidadeParaModalidadeTipoCalendario(modalidade);

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidadeTipoCalendario);

            if (tipoCalendario == null)
                return null;

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            return periodoEscolar;
        }

        private bool VerificaSeBimestreEhExpandido(IEnumerable<PeriodoEscolar> periodosEscolares, int bimestre)
        {
            var periodo = periodosEscolares.FirstOrDefault(p => p.Bimestre == bimestre);

            if (periodo == null)
                return false;

            var dataAtual = DateTime.Now;

            return periodo.PeriodoInicio <= dataAtual && periodo.PeriodoFim >= dataAtual;
        }
    }
}