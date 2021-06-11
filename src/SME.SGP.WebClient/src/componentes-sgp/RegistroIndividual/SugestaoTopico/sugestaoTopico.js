import React, { useCallback, useEffect, useState } from 'react';
import moment from 'moment';
import PropTypes from 'prop-types';

import { ServicoRegistroIndividual } from '~/servicos';

import { Container } from './sugestaoTopico.css';

const SugestaoTopico = ({ valorData }) => {
  const [textoSugestao, setTextoSugestao] = useState();

  const obterSugestao = useCallback(async () => {
    const mes = moment(valorData).format('MM');
    const mesParseado = parseInt(mes, 10);

    if (mesParseado !== 1) {
      const retorno = await ServicoRegistroIndividual.obterSugestao(
        mesParseado
      );

      if (retorno?.data) {
        setTextoSugestao(retorno?.data?.descricao);
      }
    }
  }, [valorData]);

  useEffect(() => {
    obterSugestao();
  }, [obterSugestao]);

  return (
    <>
      {textoSugestao && (
        <Container>
          <i className="fas fa-info-circle">&nbsp;Sugestão de tópico:</i>
          &nbsp;{textoSugestao}
        </Container>
      )}
    </>
  );
};

SugestaoTopico.propTypes = {
  valorData: PropTypes.string,
};

SugestaoTopico.defaultProps = {
  valorData: '',
};

export default SugestaoTopico;
