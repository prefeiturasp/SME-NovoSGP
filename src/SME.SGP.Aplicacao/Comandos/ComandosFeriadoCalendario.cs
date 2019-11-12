using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosFeriadoCalendario : IComandosFeriadoCalendario
    {
        private readonly IRepositorioFeriadoCalendario repositorio;
        private readonly IRepositorioEvento repositorioEvento;

        public ComandosFeriadoCalendario(IRepositorioFeriadoCalendario repositorio, IRepositorioEvento repositorioEvento)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public FeriadoCalendario MapearParaDominio(FeriadoCalendarioDto dto)
        {
            FeriadoCalendario entidade = repositorio.ObterPorId(dto.Id);

            if (entidade == null)
            {
                entidade = new FeriadoCalendario();
            }
            bool possuiEventos = repositorioEvento.ExisteEventoPorFeriadoId(dto.Id);
            if (!possuiEventos)
            {
                entidade.Nome = dto.Nome;
                entidade.Abrangencia = dto.Abrangencia;
                entidade.DataFeriado = dto.DataFeriado;
                entidade.Tipo = dto.Tipo;
            }
            entidade.Ativo = dto.Ativo;
            return entidade;
        }

        public void MarcarExcluidos(long[] ids)
        {
            List<string>feriadosInvalidos = new List<string>();
            List<long> idsInvalidos = new List<long>();
            foreach (long id in ids)
            {
                var feriadoCalendario = repositorio.ObterPorId(id);
                if (feriadoCalendario != null)
                {
                    var possuiEventos = repositorioEvento.ExisteEventoPorFeriadoId(id);
                    if (possuiEventos)
                    {
                        feriadosInvalidos.Add(feriadoCalendario.Nome);
                    }
                    feriadoCalendario.Excluido = true;
                    repositorio.Salvar(feriadoCalendario);
                }
                else
                {
                    idsInvalidos.Add(id);
                }
            }
            if (feriadosInvalidos.Any())
            {
                throw new NegocioException($"Houve um erro ao excluir os feriados '{string.Join(",", feriadosInvalidos.Select(n => n.ToString()).ToArray())}'. Os feriados possuem eventos vículados");
            }
            if (idsInvalidos.Any())
            {
                throw new NegocioException($"Houve um erro ao excluir os feriados id(s) '{string.Join(",", idsInvalidos.Select(n => n.ToString()).ToArray())}'. Um dos feriados não existe");
            }
        }

        public void Salvar(FeriadoCalendarioDto dto)
        {
            var feriado = MapearParaDominio(dto);

            bool ehRegistroExistente = repositorio.VerificarRegistroExistente(dto.Id, dto.Nome);
            if (ehRegistroExistente)
            {
                throw new NegocioException($"O Feriado '{dto.Nome}' já existe");
            }
            repositorio.Salvar(feriado);
        }
    }
}