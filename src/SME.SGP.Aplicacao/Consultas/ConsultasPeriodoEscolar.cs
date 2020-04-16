using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPeriodoEscolar : IConsultasPeriodoEscolar
    {
        private readonly IRepositorioPeriodoEscolar repositorio;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;

        public ConsultasPeriodoEscolar(IRepositorioPeriodoEscolar repositorio,
                                    IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                    IConsultasTipoCalendario consultasTipoCalendario)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
        }

        public PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId)
        {
            var lista = repositorio.ObterPorTipoCalendario(tipoCalendarioId);

            if (lista == null || !lista.Any())
                return null;

            return EntidadeParaDto(lista);
        }

        public DateTime ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia)
        {
            var periodos = repositorio.ObterPorTipoCalendario(tipoCalendarioId);
            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foi possível obter os períodos deste tipo de calendário.");

            DateTime fimRecorrencia = DateTime.MinValue;
            if (recorrencia == RecorrenciaAula.RepetirBimestreAtual)
            {
                // Busca ultimo dia do periodo atual
                fimRecorrencia = periodos.Where(a => a.PeriodoFim >= inicioRecorrencia)
                    .OrderBy(a => a.PeriodoInicio)
                    .FirstOrDefault().PeriodoFim;
            }
            else
            if (recorrencia == RecorrenciaAula.RepetirTodosBimestres)
            {
                // Busca ultimo dia do ultimo periodo
                fimRecorrencia = periodos.Max(a => a.PeriodoFim);
            }

            return fimRecorrencia;
        }

        public PeriodoEscolarDto ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo)
        {
            return MapearParaDto(repositorio.ObterPorTipoCalendarioData(tipoCalendarioId, dataPeriodo));
        }

        private PeriodoEscolarDto MapearParaDto(PeriodoEscolar periodo)
            => periodo == null ? null : new PeriodoEscolarDto()
            {
                Id = periodo.Id,
                Bimestre = periodo.Bimestre,
                Migrado = periodo.Migrado,
                PeriodoInicio = periodo.PeriodoInicio,
                PeriodoFim = periodo.PeriodoFim
            };

        private static PeriodoEscolarListaDto EntidadeParaDto(IEnumerable<PeriodoEscolar> lista)
        {
            return new PeriodoEscolarListaDto
            {
                TipoCalendario = lista.ElementAt(0).TipoCalendarioId,
                Periodos = lista.Select(x => new PeriodoEscolarDto
                {
                    Bimestre = x.Bimestre,
                    PeriodoInicio = x.PeriodoInicio,
                    PeriodoFim = x.PeriodoFim,
                    Migrado = x.Migrado,
                    Id = x.Id
                }).ToList()
            };
        }

        public int ObterBimestre(DateTime data, Modalidade modalidade)
        {
            var modalidadeCalendario = modalidade == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;

            var tipoCalendario = consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(data.Year, modalidadeCalendario);
            if (tipoCalendario == null)
                throw new NegocioException("Não encontrado calendario escolar cadastrado");

            var periodosEscolares = repositorio.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não encontrado periodo escolar cadastrado");

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio <= data && x.PeriodoFim >= data);

            return periodoEscolar?.Bimestre ?? 0;
        }

        public async Task<IEnumerable<PeriodoEscolarDto>> ObterPeriodosEmAberto(long ueId, Modalidade modalidadeCodigo, int anoLetivo)
        {
            var tipoCalendario = consultasTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, 
                                                                modalidadeCodigo == Modalidade.EJA ? 
                                                                    ModalidadeTipoCalendario.EJA : 
                                                                    ModalidadeTipoCalendario.FundamentalMedio);

            var periodoAtual = ObterPeriodoEscolarPorData(tipoCalendario.Id, DateTime.Now.Date);
            var periodos = new List<PeriodoEscolarDto>() { periodoAtual };
            periodos.AddRange(await consultasPeriodoFechamento.ObterPeriodosEmAberto(ueId));

            return periodos;
        }

        public async Task<PeriodoEscolarDto> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre)
            => await repositorio.ObterUltimoBimestreAsync(anoLetivo, modalidade, semestre);
    }
}