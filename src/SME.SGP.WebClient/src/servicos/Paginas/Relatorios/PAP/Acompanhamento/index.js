import api from '~/servicos/api';

const AcompanhamentoPAPServico = {
  ListarAlunos(params) {
    return api.get(`/v1/recuperacao-paralela/listar`, { params });
  },
  Salvar(dados) {
    return api.post(`/v1/recuperacao-paralela`, dados);
  },
  async ObterPeriodos(codigoTurma) {
    const retorno = await api
      .get(`/v1/recuperacao-paralela/periodo/${codigoTurma}/listar`)
      .then(res => res)
      .catch(err => err.response);

    return {
      sucesso: retorno.status === 200 || retorno.status === 204,
      semDados: retorno.status === 204,
      dados: retorno.data,
    };
  },
};

export default AcompanhamentoPAPServico;
