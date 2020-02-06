import styled from 'styled-components';
import { Base } from '~/componentes';

export const TrRegencia = styled.tr`
  vertical-align: middle;
`;

export const TdRegencia = styled.td`
  padding-left: 0 !important;
  padding-right: 0 !important;
`;

export const LinhaNotaRegencia = styled.div`
  display: flex;
`;

export const CampoNotaRegencia = styled.div`
  justify-content: center;
  align-items: center;
  margin-left: 30px;
  .disciplina{
    font-size: 11px;
    font-weight: bold;
  }
  .centro{
    justify-content: center;
    align-items: center;
    display: flex;
  }
  .nota{
    border: 1px solid ${Base.CinzaBarras};
    width: 100px;
    height: 30px;
    border-radius: 4px;
  }
`;
