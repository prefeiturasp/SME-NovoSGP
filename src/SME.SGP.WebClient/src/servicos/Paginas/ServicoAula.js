import api from '~/servicos/api';

class ServicoAula {
  salvar = async (id, aula) => {
    let metodo = 'post';
    let url = 'v1/calendarios/professores/aulas';
    if (id > 0) {
      metodo = 'put';
      url = `${url}/${id}`;
    }
    return api[metodo](url, aula);
  };
}

export default new ServicoAula();
