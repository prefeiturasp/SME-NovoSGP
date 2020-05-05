import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const DiaCompletoWrapper = styled.div`
  display: flex;
  overflow: hidden;
  max-height: 0;
  width: 100% !important;
  transition: all 0.9s;
  border-bottom: 1px solid ${Base.CinzaBordaCalendario};

  &.visivel {
    max-height: 1000px !important;
    min-height: 100px;
    height: auto !important;
    overflow: auto;
    padding: 0.3rem;
  }
`;
