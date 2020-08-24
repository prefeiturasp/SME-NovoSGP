import api from '~/servicos/api';

class ServicoNotaConceito {
  obterTodosConceitos = data => {
    return api.get(`v1/avaliacoes/notas/conceitos?data=${data}`);
  };

  obterTodasSinteses = data => {
    return api.get(`v1/avaliacoes/notas/conceitos?data=${data}`);
  };

  obterArredondamento = (nota, data) => {
    return api.get(
      `v1/avaliacoes/notas/${nota}/arredondamento?data=${data ||
        window.moment().format('YYYY-MM-DD')}`
    );
  };

  obterTipoNota = (turma, anoLetivo, consideraHistorico) => {
    return api.get(
      `v1/avaliacoes/notas/turmas/${turma}/anos-letivos/${anoLetivo}/tipos?consideraHistorico=${consideraHistorico}`
    );
  };
}
export default new ServicoNotaConceito();
