import React from 'react';
import { useSelector } from 'react-redux';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';
import { RotasDto } from '~/dtos';

const BotaoVerSituacaoEncaminhamentoAEE = () => {
  const planoAEESituacaoEncaminhamentoAEE = useSelector(
    store => store.planoAEE.planoAEESituacaoEncaminhamentoAEE
  );

  const onClick = () => {
    const win = window.open(
      `${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}/editar/${planoAEESituacaoEncaminhamentoAEE?.id}`,
      '_blank'
    );
    win.focus();
  };

  return planoAEESituacaoEncaminhamentoAEE?.id ? (
    <Button
      id="btn-voltar"
      label="Ver detalhes do encaminhamento"
      icon="eye"
      color={Colors.Azul}
      border
      className="mr-2"
      onClick={onClick}
    />
  ) : (
    ''
  );
};

export default BotaoVerSituacaoEncaminhamentoAEE;
