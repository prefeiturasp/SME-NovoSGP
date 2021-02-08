import React from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';
import { RotasDto } from '~/dtos';

const BotaoVerSituacaoEncaminhamentoAEE = () => {
  const planoAEESituacaoEncaminhamentoAEE = useSelector(
    store => store.planoAEE.planoAEESituacaoEncaminhamentoAEE
  );

  return planoAEESituacaoEncaminhamentoAEE?.id ? (
    <Link
      to={`${RotasDto.RELATORIO_AEE_ENCAMINHAMENTO}/editar/${planoAEESituacaoEncaminhamentoAEE?.id}`}
      target="_blank"
    >
      <Button
        id="btn-voltar"
        label="Ver detalhes do encaminhamento"
        icon="eye"
        color={Colors.Azul}
        border
        className="mr-2"
      />
    </Link>
  ) : (
    ''
  );
};

export default BotaoVerSituacaoEncaminhamentoAEE;
