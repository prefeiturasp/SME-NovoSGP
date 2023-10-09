using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoEscolarListaPorTipoCalendarioQueryHandler: IRequestHandler<ObterPeriodoEscolarListaPorTipoCalendarioQuery,PeriodoEscolarListaDto>
    {
        private readonly IRepositorioPeriodoEscolarConsulta repositorio;

        public ObterPeriodoEscolarListaPorTipoCalendarioQueryHandler(IRepositorioPeriodoEscolarConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PeriodoEscolarListaDto> Handle(ObterPeriodoEscolarListaPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            var lista = await repositorio.ObterPorTipoCalendario(request.TipoCalendarioId);
            if (lista.EhNulo() || !lista.Any())
                return null;
            
            return EntidadeParaDto(lista);
        }
        private static PeriodoEscolarListaDto EntidadeParaDto(IEnumerable<PeriodoEscolar> lista)
        {
            var primeiraCriacao = lista.OrderBy(x => x.CriadoEm).First();
            var ultimaAlteracao = lista.Any(x => x.AlteradoEm.HasValue) ? lista.OrderBy(x => x.AlteradoEm).Last() : null;
            return new PeriodoEscolarListaDto
            {
                TipoCalendario = lista.ElementAt(0).TipoCalendarioId,
                AlteradoEm = ultimaAlteracao?.AlteradoEm,
                AlteradoPor = ultimaAlteracao?.AlteradoPor,
                AlteradoRF = ultimaAlteracao?.AlteradoRF,
                CriadoEm = primeiraCriacao.CriadoEm,
                CriadoPor = primeiraCriacao.CriadoPor,
                CriadoRF = primeiraCriacao.CriadoRF,
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
    }
}