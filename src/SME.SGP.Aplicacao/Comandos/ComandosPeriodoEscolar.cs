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

        public ComandosPeriodoEscolar(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Salvar(PeriodoEscolarListaDto periodosDto)
        {
           
                
        }

        private PeriodoEscolarLista MapearParaDominio(PeriodoEscolarListaDto periodosDto, bool ehEja)
        {
            var periodos = periodosDto.Periodos.Select(x => new PeriodoEscolar
            {
                Id = x.Codigo,
                Bimestre = x.Bimestre,
                PeriodoInicio = x.PeriodoInicio,
                PeriodoFim = x.PeriodoFim,
                TipoCalendario = periodosDto.TipoCalendario
            }).ToList();

            return new PeriodoEscolarLista
            {
                Eja = ehEja,
                Periodos = periodos
            };
        }
    }
}
