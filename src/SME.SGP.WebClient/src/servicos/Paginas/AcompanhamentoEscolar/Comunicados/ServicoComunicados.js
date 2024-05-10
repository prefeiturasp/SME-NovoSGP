import api from '../../../api';

const TODAS_UES_ID = '-99';

class ServicoComunicados {
  listarGrupos = async () => {
    const lista = [];

    try {
      const requisicao = await api.get('v1/comunicacao/grupos/listar');
      if (requisicao.data) {
        requisicao.data.forEach(grupo => {
          lista.push({
            id: grupo.id,
            nome: grupo.nome,
          });
        });
      }
    } catch {
      return lista;
    }

    return lista;
  };

  obterIdGrupoComunicadoPorModalidade = async modalidade => {
    try {
      var retorno = await api.get(`listar/modalidade/${modalidade}`);

      return {
        sucesso: true,
        data: retorno.data,
      };
    } catch (error) {
      return {
        sucesso: false,
        erro: error,
      };
    }
  };

  consultarPorId = async id => {
    let comunicado = {};

    try {
      const requisicao = await api.get(`v1/comunicado/${id}`);
      if (requisicao.data) comunicado = requisicao.data;
    } catch {
      return comunicado;
    }

    return comunicado;
  };

  salvar = async dados => {
    let salvou = {};

    let metodo = 'post';
    let url = 'v1/comunicado';

    if (dados.id && dados.id > 0) {
      metodo = 'put';
      url = `${url}/${dados.id}`;
    }

    try {
      const requisicao = await api[metodo](url, dados);
      if (requisicao.data) salvou = requisicao;
    } catch (erro) {
      console.error('[ FAIL ] Erro ao salvar comunicado: ', erro)
      salvou = [...erro.response.data.mensagens];
    }

    return salvou;
  };

  excluir = async ids => {
    let exclusao = {};
    const parametros = { data: ids };

    try {
      const requisicao = await api.delete('v1/comunicado', parametros);
      if (requisicao && requisicao.status === 200) exclusao = requisicao;
    } catch (erro) {
      exclusao = [...erro.response.data.mensagens];
    }

    return exclusao;
  };

  buscarAnosPorModalidade = async (modalidade, codigoUe, params) => {
    return api.get(
      (codigoUe != null && codigoUe !== TODAS_UES_ID)
        ? `v1/comunicado/anos/modalidade/${modalidade}?codigoUe=${codigoUe}`
        : `v1/comunicado/anos/modalidade/${modalidade}`,
      {
        params,
      }
    );
  };

  async obterGruposPorModalidade(modalidade) {
    try {
      const requisicao = await api.get(
        `v1/comunicacao/grupos/listar/modalidade/${modalidade}`
      );

      if (requisicao && requisicao.status === 204) return [];

      return requisicao.data;
    } catch (error) {
      throw error;
    }
  }

  async obterAlunos(codigoTurma, anoLetivo) {
    try {
      const requisicao = await api.get(
        `v1/comunicado/${codigoTurma}/alunos/${anoLetivo}`
      );

      if (requisicao && requisicao.status === 204) return [];

      return requisicao.data;
    } catch (error) {
      throw error;
    }
  }
}

export default new ServicoComunicados();
