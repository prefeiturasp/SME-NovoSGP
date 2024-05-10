import api from '~/servicos/api';

// TODO Alterar quando tiver o back pronto
const urlPadrao = `/v1/compensacoes/ausencia`;

class ServicoCompensacaoAusencia {
  buscarLista = params => {
    return api.get(urlPadrao, { params });
  };

  salvar = async (id, compensacao) => {
    let url = urlPadrao;
    if (id) {
      url = `${url}/${id}`;
    }
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, compensacao);
  };

  obterPorId = async id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  deletar = async ids => {
    const parametros = { data: ids };
    return api.delete(urlPadrao, parametros);
  };

  obterAlunosComAusencia = async (turmaId, disciplinaId, bimestre) => {
    const urlAlunosComAusenciaNoBimestre = `/v1/calendarios/frequencias/ausencias/turmas/${turmaId}/disciplinas/${disciplinaId}/bimestres/${bimestre}`;
    return api.get(urlAlunosComAusenciaNoBimestre);
  };

  obterStatusCalculoFrequencia = async (turmaId, disciplinaId, bimestre) => {
    const url = `/v1/processos/executando/calculo/frequencias/turma/${turmaId}/disciplina/${disciplinaId}/bimestres/${bimestre}`;
    return api.get(url);
  };

  obterTurmasCopia = async turmaOrigemCodigo => {
    const url = `v1/compensacoes/ausencia/copiar/turmas/${turmaOrigemCodigo}`;
    return api.get(url);
  };

  copiarCompensacao = async parametros => {
    const url = `v1/compensacoes/ausencia/copiar`;
    return api.post(url, parametros);
  };
}

export default new ServicoCompensacaoAusencia();
