using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTipoCalendario : IConsultasTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;
        private readonly IRepositorioEvento repositorioEvento;

        public ConsultasTipoCalendario(IRepositorioTipoCalendario repositorio, IRepositorioEvento repositorioEvento)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public IEnumerable<TipoCalendarioDto> BuscarPorAnoLetivo(int anoLetivo)
        {
            var retorno = repositorio.BuscarPorAnoLetivo(anoLetivo);
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public TipoCalendarioCompletoDto BuscarPorAnoLetivoEModalidade(int anoLetivo, ModalidadeTipoCalendario modalidade)
        {
            var entidade = repositorio.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);

            if (entidade != null)
                return EntidadeParaDtoCompleto(entidade);

            return null;
        }

        public TipoCalendarioCompletoDto BuscarPorId(long id)
        {
            var entidade = repositorio.ObterPorId(id);

            TipoCalendarioCompletoDto dto = new TipoCalendarioCompletoDto();

            if (entidade != null)
                dto = EntidadeParaDtoCompleto(entidade);

            return dto;
        }

        public TipoCalendarioDto EntidadeParaDto(TipoCalendario entidade)
        {
            return new TipoCalendarioDto()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Modalidade = entidade.Modalidade,
                DescricaoPeriodo = entidade.Periodo.GetAttribute<DisplayAttribute>().Name,
                Periodo = entidade.Periodo,
                Migrado = entidade.Migrado
            };
        }

        public TipoCalendarioCompletoDto EntidadeParaDtoCompleto(TipoCalendario entidade)
        {
            bool possuiEventos = repositorioEvento.ExisteEventoPorTipoCalendarioId(entidade.Id);
            return new TipoCalendarioCompletoDto
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Periodo = entidade.Periodo,
                Modalidade = entidade.Modalidade,
                Situacao = entidade.Situacao,
                AlteradoPor = entidade.AlteradoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoRF = entidade.AlteradoRF,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor,
                DescricaoPeriodo = entidade.Periodo.GetAttribute<DisplayAttribute>().Name,
                PossuiEventos = possuiEventos
            };
        }

        public IEnumerable<TipoCalendarioDto> Listar()
        {
            var retorno = repositorio.ObterTiposCalendario();
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public IEnumerable<TipoCalendarioDto> ListarPorAnoLetivo(int anoLetivo)
        {
            var retorno = repositorio.ListarPorAnoLetivo(anoLetivo);
            return from t in retorno
                   select EntidadeParaDto(t);
        }

        public async Task<TipoCalendario> ObterPorTurma(Turma turma, DateTime dataReferencia)
            => repositorio.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo
                    , turma.ModalidadeTipoCalendario
                    , dataReferencia.Semestre());

        public Task<bool> PeriodoEmAberto(TipoCalendario tipoCalendario, DateTime dataReferencia)
            => repositorio.PeriodoEmAberto(tipoCalendario.Id, dataReferencia);
    }
}