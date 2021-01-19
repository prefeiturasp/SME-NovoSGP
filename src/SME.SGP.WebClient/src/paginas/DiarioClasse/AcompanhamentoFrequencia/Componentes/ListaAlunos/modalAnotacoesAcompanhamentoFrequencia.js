import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Card } from 'antd';
import { Colors, DetalhesAluno, ModalConteudoHtml } from '~/componentes';
import Button from '~/componentes/button';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';

const ModalAnotacoesAcompanhamentoFrequencia = () => {
  const dispatch = useDispatch();

  const dadosModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.dadosModalAnotacao
  );

  const exibirModalAnotacao = useSelector(
    store => store.acompanhamentoFrequencia.exibirModalAnotacao
  );

  const onClose = () => {
    dispatch(setDadosModalAnotacao());
    dispatch(setExibirModalAnotacao(false));
  };
  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="anotacao"
      visivel={exibirModalAnotacao}
      titulo="Anotações do Aluno"
      onClose={onClose}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <DetalhesAluno className="mb-2" />
      <Card
        type="inner"
        className="rounded mt-3 mb-3"
        headStyle={{ borderBottomRightRadius: 0 }}
        bodyStyle={{ borderTopRightRadius: 0 }}
      >
        <strong>{dadosModalAnotacao?.motivo}</strong>
      </Card>
      <JoditEditor
        value={dadosModalAnotacao?.anotacao}
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

export default ModalAnotacoesAcompanhamentoFrequencia;
