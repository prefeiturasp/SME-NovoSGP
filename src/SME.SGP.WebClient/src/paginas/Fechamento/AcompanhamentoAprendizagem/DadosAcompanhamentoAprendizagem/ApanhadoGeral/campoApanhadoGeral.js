import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { JoditEditor } from '~/componentes';
import { setApanhadoGeralEmEdicao } from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';

const CampoApanhadoGeral = () => {
  const dispatch = useDispatch();

  const dadosApanhadoGeral = useSelector(
    store => store.acompanhamentoAprendizagem.dadosApanhadoGeral
  );

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  const onChange = valorNovo => {
    ServicoAcompanhamentoAprendizagem.atualizarApanhadoGeral(valorNovo);
    dispatch(setApanhadoGeralEmEdicao(true));
  };

  return (
    <JoditEditor
      id="apanhado-geral-editor"
      value={dadosApanhadoGeral?.apanhadoGeral}
      onChange={onChange}
      readonly={desabilitarCamposAcompanhamentoAprendizagem}
    />
  );
};

export default CampoApanhadoGeral;
