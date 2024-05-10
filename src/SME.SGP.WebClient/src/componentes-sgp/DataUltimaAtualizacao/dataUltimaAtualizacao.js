import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerDataUltimaAtualizacao = styled.span`
  background-color: ${Base.Azul};
  border-radius: 4px;
  color: ${Base.Branco};
  font-weight: bold;
  padding: 3px 5px 3px 5px;
  font-size: 12px;
`;

const DataUltimaAtualizacao = props => {
  const { dataFormatada, descricaoAdicional } = props;

  return dataFormatada ? (
    <div className="d-flex justify-content-end pb-4">
      <ContainerDataUltimaAtualizacao>
        <div>Data da última atualização: {dataFormatada}</div>
        {descricaoAdicional ? <div>{descricaoAdicional}</div> : ''}
      </ContainerDataUltimaAtualizacao>
    </div>
  ) : (
    ''
  );
};

DataUltimaAtualizacao.propTypes = {
  dataFormatada: PropTypes.string,
  descricaoAdicional: PropTypes.string,
};

DataUltimaAtualizacao.defaultProps = {
  dataFormatada: '',
  descricaoAdicional: '',
};

export default DataUltimaAtualizacao;
