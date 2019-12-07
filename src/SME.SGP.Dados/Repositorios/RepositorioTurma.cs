using Dapper;
using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        private const string QuerySincronizacao = @"
                    select
	                    id,
	                    turma_id,
	                    ue_id,
	                    nome,
	                    ano,
	                    ano_letivo,
	                    modalidade_codigo,
	                    semestre,
	                    qt_duracao_aula,
	                    tipo_turno,
	                    data_atualizacao
                    from
	                    public.turma
                    where turma_id in (#ids);";

        private const string Update = @"
                    update
	                    public.turma
                    set
	                    nome = @nome,
	                    ano = @ano,
	                    ano_letivo = @anoLetivo,
	                    modalidade_codigo = @modalidadeCodigo,
	                    semestre = @semestre,
	                    qt_duracao_aula = @qtDuracaoAula,
	                    tipo_turno = @tipoTurno,
	                    data_atualizacao = @dataAtualizacao
                    where
	                    id = @id;";

        private readonly ISgpContext contexto;

        public RepositorioTurma(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public Turma ObterPorId(string turmaId)
        {
            return contexto.QueryFirstOrDefault<Turma>("select * from turma where turma_id = @turmaId", new { turmaId });
        }

        public IEnumerable<Turma> Sincronizar(IEnumerable<Turma> entidades, IEnumerable<Ue> ues)
        {
            List<Turma> resultado = new List<Turma>();

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Turma>(QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoTurma}'")))).ToList();

                var novos = iteracao.Where(x => !armazenados.Select(y => y.CodigoTurma).Contains(x.CodigoTurma)).ToList();

                foreach (var item in novos)
                {
                    item.DataAtualizacao = DateTime.Today;
                    item.Ue = ues.First(x => x.CodigoUe == item.Ue.CodigoUe);
                    item.UeId = item.Ue.Id;
                    item.Id = (long)contexto.Conexao.Insert(item);
                    resultado.Add(item);
                }

                var modificados = from c in entidades
                                  join l in armazenados on c.CodigoTurma equals l.CodigoTurma
                                  where l.DataAtualizacao != DateTime.Today &&
                                        (c.Nome != l.Nome ||
                                        c.Ano != l.Ano ||
                                        c.AnoLetivo != l.AnoLetivo ||
                                        c.ModalidadeCodigo != l.ModalidadeCodigo ||
                                        c.Semestre != l.Semestre ||
                                        c.QuantidadeDuracaoAula != l.QuantidadeDuracaoAula ||
                                        c.TipoTurno != l.TipoTurno)
                                  select new Turma()
                                  {
                                      Ano = c.Ano,
                                      AnoLetivo = c.AnoLetivo,
                                      CodigoTurma = c.CodigoTurma,
                                      DataAtualizacao = DateTime.Today,
                                      Id = l.Id,
                                      ModalidadeCodigo = c.ModalidadeCodigo,
                                      Nome = c.Nome,
                                      QuantidadeDuracaoAula = c.QuantidadeDuracaoAula,
                                      Semestre = c.Semestre,
                                      TipoTurno = c.TipoTurno,
                                      Ue = l.Ue,
                                      UeId = l.UeId
                                  };

                foreach (var item in modificados)
                {
                    contexto.Conexao.Execute(Update, new
                    {
                        nome = item.Nome,
                        ano = item.Ano,
                        anoLetivo = item.AnoLetivo,
                        modalidadeCodigo = item.ModalidadeCodigo,
                        semestre = item.Semestre,
                        qtDuracaoAula = item.QuantidadeDuracaoAula,
                        tipoTurno = item.TipoTurno,
                        dataAtualizacao = item.DataAtualizacao,
                        id = item.Id
                    });

                    resultado.Add(item);
                }

                resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoTurma).Contains(x.CodigoTurma)));
            }

            return resultado;
        }
    }
}