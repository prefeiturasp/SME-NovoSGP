import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

import { statusAcompanhamentoFechamento } from '~/dtos';

import { Container } from './cardStatus.css';

const CardStatus = ({ dadosStatus }) => {
  const [corStatus, setCorStatus] = useState('');

  useEffect(() => {
    const status = Object.keys(statusAcompanhamentoFechamento).find(
      item =>
        statusAcompanhamentoFechamento[item].descricao === dadosStatus.descricao
    );

    if (status) {
      setCorStatus(statusAcompanhamentoFechamento[status].cor);
    }
  }, [dadosStatus]);

  return (
    <Container corStatus={corStatus}>
      <div className="descricao">{dadosStatus.descricao}</div>
      <div className="quantidade">{dadosStatus.quantidade}</div>
    </Container>
  );
};

CardStatus.propTypes = {
  dadosStatus: PropTypes.oneOfType([PropTypes.object, PropTypes.array]),
};

CardStatus.defaultProps = {
  dadosStatus: [],
};
export default CardStatus;
