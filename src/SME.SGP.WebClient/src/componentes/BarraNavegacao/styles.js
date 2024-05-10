import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const Container = styled.div`
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: relative;

  .btn.attached.right {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
    position: relative !important;
    right: 0;
  }

  .btn.attached.left {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
    position: relative !important;
    left: 0;
  }

  .conteudo {
    flex: 1;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: ${Base.CinzaFundo};
    border: 1px solid ${Base.CinzaDesabilitado};
    border-left: 1px solid ${Base.CinzaBadge};
    border-right: 1px solid ${Base.CinzaBadge};
    height: 38px;

    .ttpItemNavegacao {
      cursor: pointer;
    }

    .itemNavegacao {
      width: 10px;
      height: 10px;
      background-color: ${Base.CinzaDesabilitado};
      margin: 0 0.2rem;
      border-radius: 50%;
      cursor: pointer;

      &.ativo,
      &:hover {
        background-color: ${Base.Roxo};
      }
    }
  }
`;
