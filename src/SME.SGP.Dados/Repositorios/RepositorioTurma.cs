using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dommel;
using System.Linq;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurma : IRepositorioTurma
    {
        const string QuerySincronizacao = @"
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
        const string Update = @"
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
        private readonly IRepositorioUe respositorioUe;

        public RepositorioTurma(ISgpContext contexto, IRepositorioUe respositorioUe)
        {
            this.contexto = contexto;
            this.respositorioUe = respositorioUe;
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

                foreach (var item in armazenados)
                {
                    var entidade = iteracao.First(x => x.CodigoTurma == item.CodigoTurma);
                    entidade.Id = item.Id;
                    entidade.Ue = item.Ue;
                    entidade.UeId = item.UeId;

                    if (item.DataAtualizacao.Date != DateTime.Today)
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
                            dataAtualizacao = DateTime.Today,
                            id = item.Id
                        });
                    }

                    resultado.Add(entidade);
                }
            }

            return resultado;
        }

    }

}
