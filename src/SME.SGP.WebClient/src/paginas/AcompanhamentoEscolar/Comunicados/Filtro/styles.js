import styled from 'styled-components';

export const Row = styled.div`
  [class*='col-'] {
    padding: 0 8px !important;
    @media screen and (min-width: 0px) and (max-width: 900px) {
      padding: 0 !important;
    }
  }

  [class*='col-']:first-child {
    padding-left: 0px !important;
  }

  [class*='col-']:last-child {
    padding-right: 0px !important;
  }

  @media screen and (min-width: 0px) and (max-width: 900px) {
    padding: 0 !important;
    margin-bottom: 0 !important;
    margin-top: 0 !important;
  }
`;
