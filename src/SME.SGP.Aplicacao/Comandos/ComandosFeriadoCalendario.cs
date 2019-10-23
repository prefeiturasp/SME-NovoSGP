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

        public ComandosFeriadoCalendario(IRepositorioFeriadoCalendario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public FeriadoCalendario MapearParaDominio(FeriadoCalendarioDto dto)
        {
            FeriadoCalendario entidade = repositorio.ObterPorId(dto.Id);
            if (entidade == null)
            {
                entidade = new FeriadoCalendario();
            }
            entidade.Nome = dto.Nome;
            entidade.Abrangencia = dto.Abrangencia;
            entidade.Ativo = dto.Ativo;
            entidade.DataFeriado = dto.DataFeriado;
            entidade.Tipo = dto.Tipo;
            return entidade;
        }

        public void MarcarExcluidos(long[] ids)
        {
            List<long> idsInvalidos = new List<long>();
            foreach (long id in ids)
            {
                var feriadoCalendario = repositorio.ObterPorId(id);
                if (feriadoCalendario != null)
                {
                    feriadoCalendario.Excluido = true;
                    repositorio.Salvar(feriadoCalendario);
                }
                else
                {
                    idsInvalidos.Add(id);
                }
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