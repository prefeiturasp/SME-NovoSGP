using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosPeriodoEscolar : IComandosPeriodoEscolar
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConsultasTipoCalendario consultaTipoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodo;

        public ComandosPeriodoEscolar(IUnitOfWork unitOfWork, IConsultasTipoCalendario consultaTipoCalendario, IRepositorioPeriodoEscolar repositorioPeriodo)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.consultaTipoCalendario = consultaTipoCalendario ?? throw new ArgumentNullException(nameof(consultaTipoCalendario));
            this.repositorioPeriodo = repositorioPeriodo ?? throw new ArgumentException(nameof(repositorioPeriodo));
        }

        public void Salvar(PeriodoEscolarListaDto periodosDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                TipoCalendarioCompletoDto tipo = consultaTipoCalendario.BuscarPorId(periodosDto.TipoCalendario);

                if (tipo == null || tipo.Id == 0) throw new NegocioException("O tipo de calendario informado não foi encontrado");

                var lista = new PeriodoEscolarLista
                {
                    AnoBase = periodosDto.AnoBase,
                    Eja = tipo.Modalidade == Modalidade.EJA
                };

                lista = MapearListaPeriodos(periodosDto, lista);

                lista.Validar();
            }
        }

        private PeriodoEscolarLista MapearListaPeriodos(PeriodoEscolarListaDto periodosDto, PeriodoEscolarLista lista)
        {
            foreach (var periodo in periodosDto.Periodos)
            {
                PeriodoEscolar periodoSalvar = null;

                if (periodo.Codigo > 0)
                    periodoSalvar = ObterPeriodo(periodo.Codigo, periodo);
                else
                    periodoSalvar = MapearParaDominio(periodo, periodosDto.TipoCalendario);

                lista.Periodos.Add(periodoSalvar);
            }

            return lista;
        }

        private PeriodoEscolar ObterPeriodo(long codigo, PeriodoEscolarDto periodo)
        {
            var periodoSalvar = new PeriodoEscolar();

            periodoSalvar.PeriodoInicio = periodo.PeriodoInicio;
            periodoSalvar.PeriodoFim = periodo.PeriodoFim;

            return periodoSalvar;
        }

        private PeriodoEscolar MapearParaDominio(PeriodoEscolarDto periodoDto, long tipoCalendario)
        {
            return new PeriodoEscolar
            {
                Id = periodoDto.Codigo,
                Bimestre = periodoDto.Bimestre,
                PeriodoInicio = periodoDto.PeriodoInicio,
                PeriodoFim = periodoDto.PeriodoFim,
                TipoCalendario = tipoCalendario
            };
        }
    }
}
