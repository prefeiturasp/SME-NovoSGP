import PropTypes from 'prop-types';
import React, { useState } from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { LinhaObservacao, CampoObservacao } from './observacoesChat.css';

const LinhaObservacaoProprietario = props => {
  const { observacao } = props;

  const [modoEdicao, setModoEdicao] = useState(false);
  const [valorObservacao, setValorObservacao] = useState(observacao.texto);

  const onClickEditar = () => {
    console.log(observacao);
    setModoEdicao(true);
  };

  const onClickExcluir = () => {
    console.log(observacao);
  };

  const onClickSalvar = () => {
    console.log(observacao);
  };

  const onClickCancelar = () => {
    console.log(observacao);
  };

  const onChangeObs = ({ target: { value } }) => {
    observacao.texto = value;
    setValorObservacao(value);
  };

  const btnSalvarCancelar = () => {
    return (
      <div className="d-flex mt-2">
        <Button
          id="btn-cancelar-obs-novo"
          label="Cancelar"
          color={Colors.Roxo}
          border
          bold
          className="mr-3"
          onClick={onClickCancelar}
          height="25px"
          hidden={!modoEdicao}
        />
        <Button
          id="btn-salvar-obs-novo"
          label="Salvar"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickSalvar}
          height="25px"
          hidden={!modoEdicao}
        />
      </div>
    );
  };

  const btnEditarExcluir = () => {
    return (
      <div className="d-flex">
        <Button
          id="btn-editar"
          icon="edit"
          iconType="far"
          color={Colors.Azul}
          border
          className="btn-acao mr-2"
          onClick={onClickEditar}
          height="30px"
          width="30px"
        />
        <Button
          id="btn-excluir"
          icon="trash-alt"
          iconType="far"
          color={Colors.Azul}
          border
          className="btn-acao"
          onClick={onClickExcluir}
          height="30px"
          width="30px"
        />
      </div>
    );
  };

  return (
    <div className="row">
      <div className="col-md-4" />
      {modoEdicao ? (
        <div className="col-md-8 mb-5">
          <CampoObservacao
            id="editando-observacao"
            autoSize={{ minRows: 4 }}
            value={valorObservacao}
            onChange={onChangeObs}
          />
          {btnSalvarCancelar()}
        </div>
      ) : (
        <LinhaObservacao className="col-md-8 mb-5">
          <div>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
            eiusmod tempor incididunt ut labore et dolore reprehenderit in
            voluptate velit esse cillum dolore Lorem ipsum dolor sit amet,
            consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
            labore et dolore reprehenderit in voluptate velit esse cillum dolore
          </div>
          {btnEditarExcluir()}
        </LinhaObservacao>
      )}
    </div>
  );
};

LinhaObservacaoProprietario.propTypes = {
  observacao: PropTypes.oneOfType([PropTypes.object]),
};

LinhaObservacaoProprietario.defaultProps = {
  observacao: {},
};

export default LinhaObservacaoProprietario;
