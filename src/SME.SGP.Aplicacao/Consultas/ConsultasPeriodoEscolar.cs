using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPeriodoEscolar : IConsultasPeriodoEscolar
    {
        private readonly IRepositorioPeriodoEscolar repositorio;

        public ConsultasPeriodoEscolar(IRepositorioPeriodoEscolar repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public PeriodoEscolarListaDto ObterPorTipoCalendario(long codigoTipoCalendario)
        {
            var lista = repositorio.ObterPorTipoCalendario(codigoTipoCalendario);

            if (lista == null || lista.Count == 0)
                return null;

            return EntidadeParaDto(lista);
        }

        private static PeriodoEscolarListaDto EntidadeParaDto(IList<Dominio.Entidades.PeriodoEscolar> lista)
        {
            return new PeriodoEscolarListaDto
            {
                AnoBase = lista[0].PeriodoInicio.Year,
                TipoCalendario = lista[0].TipoCalendario,
                Periodos = lista.Select(x => new PeriodoEscolarDto
                {
                    Bimestre = x.Bimestre,
                    PeriodoInicio = x.PeriodoInicio,
                    PeriodoFim = x.PeriodoFim,
                    Codigo = x.Id
                }).ToList()
            };
        }
    }
}
