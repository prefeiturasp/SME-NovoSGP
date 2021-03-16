import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';

const BotoesAcoesAcompanhamentoAprendizagem = () => {
  const onClickVoltar = async () => {
    history.push(URL_HOME);
  };

  const onClickCancelar = async () => {};

  const onClickSalvar = async () => {};

  return (
    <>
      <Button
        id="btn-voltar"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onClickCancelar}
      />
      <Button
        id="btn-salvar"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onClickSalvar}
      />
    </>
  );
};

export default BotoesAcoesAcompanhamentoAprendizagem;
