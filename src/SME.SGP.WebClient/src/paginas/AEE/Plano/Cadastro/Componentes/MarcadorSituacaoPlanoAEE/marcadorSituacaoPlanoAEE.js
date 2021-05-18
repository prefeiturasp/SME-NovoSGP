import React from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Base } from '~/componentes';

export const SituacaoAEEInfo = styled.div`
  background-color: ${Base.Roxo};
  color: ${Base.Branco};
  padding: 4px;
  font-size: 12px;
  border-radius: 4px;
  span {
    margin: 10px;
  }
`;

const MarcadorSituacaoPlanoAEE = () => {
  const dadosPlanoAEE = useSelector(store => store.planoAEE.planoAEEDados);
  return dadosPlanoAEE?.situacaoDescricao ? (
    <SituacaoAEEInfo>{dadosPlanoAEE?.situacaoDescricao}</SituacaoAEEInfo>
  ) : (
    ''
  );
};

export default MarcadorSituacaoPlanoAEE;
