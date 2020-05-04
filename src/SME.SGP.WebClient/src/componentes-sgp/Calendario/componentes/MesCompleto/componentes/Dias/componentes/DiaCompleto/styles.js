import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const DiaCompletoWrapper = styled.div`
  min-width: 100%;
  display: flex;
  overflow: hidden;
  max-height: 0;
  width: 100% !important;
  transition: all 0.9s;
  border-bottom: 1px solid ${Base.CinzaBordaCalendario};

  &.visivel {
    max-height: 1000px !important;
    min-height: 200px;
    height: auto !important;
    overflow: auto;
  }
`;
