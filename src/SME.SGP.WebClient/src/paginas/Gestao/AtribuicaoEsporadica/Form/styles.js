import styled from 'styled-components';

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

  margin: 0;
  margin-bottom: 16px;
`;
