import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes/colors';

export const FundoCinza = styled.div`
  display: flex !important;
  flex-wrap: wrap !important;

  .border {
    border-color: ${Base.CinzaBordaCalendario} !important;
  }

  .badge-light {
    background-color: ${Base.CinzaBadge} !important;
  }
`;
