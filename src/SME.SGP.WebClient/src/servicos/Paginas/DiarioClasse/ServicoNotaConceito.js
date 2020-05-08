import api from '~/servicos/api';

class ServicoNotaConceito {
  obterTodosConceitos = data => {
    return api.get(`v1/avaliacoes/notas/conceitos?data=${data}`);
  };

  obterArredondamento = (nota, data) => {
    return api.get(
      `v1/avaliacoes/notas/${nota}/arredondamento?data=${data ||
        window.moment().format('YYYY-MM-DD')}`
    );
  };
}
export default new ServicoNotaConceito();
