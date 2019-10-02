import styled from 'styled-components';
import Card from '~/componentes/card';

export const EstiloLista = styled(Card)`
  margin-top: 0.5rem !important;

  div[class^='col-'] {
    padding-left: 6px !important;
    padding-right: 6px !important;
  }

  .data-hora {
    line-height: 1rem;
    white-space: normal !important;
  }
`;
