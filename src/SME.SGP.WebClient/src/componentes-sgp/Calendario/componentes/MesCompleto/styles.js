import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes/colors';

export const MesCompletoWrapper = styled.div`
  display: flex;
  overflow: hidden;
  max-height: 0;
  width: 100% !important;
  transition: all 0.9s;
  border-left: 1px solid ${Base.CinzaBordaCalendario};
  border-right: 1px solid ${Base.CinzaBordaCalendario};

  &.visivel {
    max-height: 1000px !important;
    height: auto !important;
    overflow: auto;
  }
`;
