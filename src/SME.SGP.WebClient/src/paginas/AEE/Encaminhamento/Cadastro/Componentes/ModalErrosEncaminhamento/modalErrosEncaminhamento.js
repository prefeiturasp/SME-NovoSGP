import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import { setExibirModalErrosEncaminhamento } from '~/redux/modulos/encaminhamentoAEE/actions';

function ModalErrosEncaminhamento() {
  const dispatch = useDispatch();

  const exibirModalErrosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.exibirModalErrosEncaminhamento
  );

  const nomesSecoesComCamposObrigatorios = useSelector(
    store => store.encaminhamentoAEE.nomesSecoesComCamposObrigatorios
  );

  let mensagemErro = `Existem campos obrigatórios sem preenchimentos nas seguintes seções: `;

  nomesSecoesComCamposObrigatorios.forEach((secaoNome, index) => {
    mensagemErro += secaoNome;
    if (index + 1 < nomesSecoesComCamposObrigatorios.length) {
      mensagemErro += ' / ';
    }
  });

  const onCloseErros = () => {
    dispatch(setExibirModalErrosEncaminhamento(false));
  };

  return (
    <ModalMultiLinhas
      key="erros-encaminhamento"
      visivel={exibirModalErrosEncaminhamento}
      onClose={onCloseErros}
      type="error"
      conteudo={[mensagemErro]}
      titulo="Atenção"
    />
  );
}

export default ModalErrosEncaminhamento;
