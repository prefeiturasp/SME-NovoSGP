using System;
using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAula : IConsultasAula
    {
        private readonly IRepositorioAula repositorio;

        public ConsultasAula(IRepositorioAula repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public AulaConsultaDto BuscarPorId(long id)
        {
            var aula = repositorio.ObterPorId(id);
            return MapearParaDto(aula);
        }

        private AulaConsultaDto MapearParaDto(Aula aula)
        {
            AulaConsultaDto dto = new AulaConsultaDto()
            {
                Id = aula.Id,
                DisciplinaId = aula.DisciplinaId,
                TurmaId = aula.TurmaId,
                TipoCalendarioId = aula.TipoCalendarioId,
                TipoAula = aula.TipoAula,
                Quantidade = aula.Quantidade,
                DataAula = aula.DataAula,
                RecorrenciaAula = aula.RecorrenciaAula,
                AlteradoEm = aula.AlteradoEm,
                AlteradoPor = aula.AlteradoPor,
                AlteradoRF = aula.AlteradoRF,
                CriadoEm = aula.CriadoEm,
                CriadoPor = aula.CriadoPor,
                CriadoRF = aula.CriadoRF
            };
            return dto;
        }

    }
}
