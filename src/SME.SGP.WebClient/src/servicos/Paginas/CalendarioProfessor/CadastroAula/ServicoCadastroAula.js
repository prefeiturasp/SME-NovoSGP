import api from '~/servicos/api';

class ServicoCadastroAula {
  obterPorId = idAula => {
    const url = `v1/calendarios/professores/aulas/${idAula}`;
    return api.get(url);
  };

  salvar = aula => {
    return api.post('v1/calendarios/professores/aulas/', aula);
  };
}

export default new ServicoCadastroAula();
