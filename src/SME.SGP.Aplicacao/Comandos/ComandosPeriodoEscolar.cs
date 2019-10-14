using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
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
        private readonly IConsultasTipoCalendario tipoCalendario;

        public ComandosPeriodoEscolar(IUnitOfWork unitOfWork, IConsultasTipoCalendario tipoCalendario)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.tipoCalendario = tipoCalendario ?? throw new ArgumentNullException(nameof(tipoCalendario));
        }

        public void Salvar(PeriodoEscolarListaDto periodosDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                TipoCalendarioCompletoDto tipo = tipoCalendario.BuscarPorId(periodosDto.TipoCalendario);

                if (tipo.Id == 0) throw new NegocioException("O tipo de calendario informado não foi encontrado");

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
