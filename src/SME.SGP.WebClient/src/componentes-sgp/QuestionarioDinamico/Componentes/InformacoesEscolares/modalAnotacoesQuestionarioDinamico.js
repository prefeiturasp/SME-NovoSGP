import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Colors, ModalConteudoHtml } from '~/componentes';
import Button from '~/componentes/button';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setQuestionarioDinamicoDadosModalAnotacao,
  setQuestionarioDinamicoExibirModalAnotacao,
} from '~/redux/modulos/questionarioDinamico/actions';

const ModalAnotacoesQuestionarioDinamico = () => {
  const dispatch = useDispatch();

  const dadosModalAnotacao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoDadosModalAnotacao
  );

  const exibirModalAnotacao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoExibirModalAnotacao
  );

  const onClose = () => {
    dispatch(setQuestionarioDinamicoDadosModalAnotacao());
    dispatch(setQuestionarioDinamicoExibirModalAnotacao(false));
  };
  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="anotacao"
      visivel={exibirModalAnotacao}
      titulo="Anotações"
      onClose={onClose}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <JoditEditor
        value={dadosModalAnotacao?.justificativaAusencia}
        readonly
        removerToolbar
      />
      <div className="col-md-12 mt-2 p-0 d-flex justify-content-end">
        <Button
          key="btn-voltar"
          id="btn-voltar"
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          onClick={onClose}
          className="mt-2"
        />
      </div>
    </ModalConteudoHtml>
  );
};

export default ModalAnotacoesQuestionarioDinamico;
