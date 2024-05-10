import styled from 'styled-components';
import { Base } from '~/componentes';

export const TrRegencia = styled.tr`
  td {
    vertical-align: middle;
  }
  .destaque-label {
    box-shadow: -5px 0px 9px 5px rgba(0, 0, 0, 0.095);
  }
`;

export const TdRegencia = styled.td`
  box-shadow: 6px 0px 9px 5px rgba(0, 0, 0, 0.095);
  margin-left: 30px;
  max-width: 300px;
  overflow-x: auto;
  padding-left: 0 !important;
  padding-right: 0 !important;
`;

export const LinhaNotaRegencia = styled.div`
  display: flex;
`;

export const CampoNotaRegencia = styled.div`
  justify-items: center;
  align-items: center;
  display: grid;
  margin-left: 30px;
  .disciplina {
    font-size: 11px;
    font-weight: bold;
  }
  .centro {
    justify-content: center;
    align-items: center;
    display: flex;
  }
  .nota {
    border: 1px solid ${Base.CinzaBarras};
    width: 100px;
    height: 30px;
    border-radius: 4px;
  }
`;
