import React, { useCallback, useEffect, useState } from 'react';
import moment from 'moment';
import PropTypes from 'prop-types';

import { ServicoRegistroIndividual } from '~/servicos';

import { Container } from './sugestaoTopico.css';

const SugestaoTopico = ({ valorData }) => {
  const [textoSugestao, setTextoSugestao] = useState();

  const obterSugestao = useCallback(async () => {
    const mesSelecionado = moment(valorData).format('MM');
    const retorno = await ServicoRegistroIndividual.obterSugestao({
      mes: mesSelecionado,
    });

    if (retorno) {
      setTextoSugestao(retorno.textoSugestao);
    }
  }, [valorData]);

  useEffect(() => {
    obterSugestao();
  }, [obterSugestao]);

  return (
    <Container>
      <i className="fas fa-info-circle">&nbsp;Sugestão de tópico:</i>
      &nbsp;{textoSugestao}
    </Container>
  );
};

SugestaoTopico.propTypes = {
  valorData: PropTypes.string,
};

SugestaoTopico.defaultProps = {
  valorData: '',
};

export default SugestaoTopico;
