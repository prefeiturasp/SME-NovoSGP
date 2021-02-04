import React from 'react';
import { useSelector } from 'react-redux';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';

const BotaoVerSituacaoEncaminhamentoAEE = () => {
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const onClick = () => {
    return '';
  };

  return planoAEEDados?.encaminhamento ? (
    <>
      <Button
        id="btn-voltar"
        label="Ver detalhes do encaminhamento"
        icon="eye"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClick}
      />
    </>
  ) : (
    ''
  );
};

export default BotaoVerSituacaoEncaminhamentoAEE;
