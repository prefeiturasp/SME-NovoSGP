import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes/colors';

const DivWrapperMes = styled.div`
  display: flex;
  width: 25% !important;
  border: 1px solid ${Base.CinzaBordaCalendario} !important;

  &:nth-child(-n + 3),
  &:nth-child(n + 5):nth-child(-n + 8),
  &:nth-child(n + 10):nth-child(-n + 13) {
    border-right: 0 !important;
  }

  &:nth-child(n + 5):nth-child(-n + 9),
  &:nth-child(n + 11):nth-child(-n + 14) {
    border-top: 0 !important;
  }

  &.aberto {
    border-bottom: none !important;
    .seta {
      background-color: ${Base.Branco} !important;

      .iconeSeta {
        color: ${Base.Preto};
        transform: rotate(90deg);
      }
    }
  }
`;

const DivMes = styled.div`
  width: 100% !important;
  display: flex;
  color: ${Base.Preto};

  .seta {
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    height: 75px;
    width: 35px;
    color: ${Base.Branco};
    background-color: ${Base.AzulCalendario};

    .iconeSeta {
      transition: all 0.5s !important;
    }
  }
`;

export { DivWrapperMes, DivMes };
