import styled from 'styled-components';

export const CaixaAno = styled.div`
  width: 100%;
  border-radius: 3px;
  background-color: #dadada;
  text-align: center;
  padding: 10px;
`;

export const CaixaTextoAno = styled.div`
  color: #42474a;
`;

export const Row = styled.div`
  [class*='col-'] {
    padding: 0 8px !important;
  }

  [class*='col-']:first-child {
    padding-left: 0px !important;
  }

  [class*='col-']:last-child {
    padding-right: 0px !important;
  }
`;
