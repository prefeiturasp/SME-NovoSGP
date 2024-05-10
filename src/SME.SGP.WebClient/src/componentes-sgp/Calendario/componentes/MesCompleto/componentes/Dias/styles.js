import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const DiasWrapper = styled.div`
  display: flex !important;
  flex-wrap: wrap !important;
  cursor: pointer !important;
`;

export const IconeDiaComPendencia = styled.i`
  font-size: 15px;
  color: ${Base.LaranjaAlerta};
`;
