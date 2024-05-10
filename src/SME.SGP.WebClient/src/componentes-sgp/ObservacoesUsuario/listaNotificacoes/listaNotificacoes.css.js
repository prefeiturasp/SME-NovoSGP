import styled, { css } from 'styled-components';

export const Container = styled.div`
  position: ${({ somenteLeitura }) => !somenteLeitura && 'absolute'};

  ${({ listagemDiario }) =>
    listagemDiario
      ? css`
          bottom: ${({ temLinhaAlteradoPor }) =>
            temLinhaAlteradoPor ? '-25px' : '-6px'};
        `
      : css`
          bottom: ${({ temLinhaAlteradoPor }) =>
            temLinhaAlteradoPor ? '-19px' : '1px'};
        `};

  span {
    font-family: Roboto;
    font-size: 9px;
    font-weight: bold;
    color: #42474a;
  }
`;
