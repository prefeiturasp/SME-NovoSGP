import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';

const BotoesAcoesAcompanhamentoAprendizagem = () => {
  const onClickVoltar = async () => {
    history.push(URL_HOME);
  };

  return (
    <Button
      id="btn-voltar"
      label="Voltar"
      icon="arrow-left"
      color={Colors.Azul}
      border
      onClick={onClickVoltar}
    />
  );
};

export default BotoesAcoesAcompanhamentoAprendizagem;
