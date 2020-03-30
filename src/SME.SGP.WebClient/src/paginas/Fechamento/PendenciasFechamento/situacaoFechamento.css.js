import styled from 'styled-components';
import { Input } from 'antd';
const { TextArea } = Input;

export const ResolvidoForm = styled.div`
  width: 144px;
  border-radius: 4px;
  display: inline-grid;
  padding: 10px;
  margin-top: 24px;
  justify-items: center;
  color: #d06d12;
  background-color: #fff2e8;
  border: solid 1px #eec4ad;
`;
export const AprovadoForm = styled.div`
  width: 144px;
  border-radius: 4px;
  display: inline-grid;
  padding: 10px;
  margin-top: 24px;
  justify-items: center;
  color: #198459;
  border: solid 1px #90ceb5;
  background-color: #f6ffed;
`;

export const PendenteForm = styled.div`
  width: 144px;
  border-radius: 4px;
  display: inline-grid;
  padding: 10px;
  margin-top: 24px;
  justify-items: center;
  color: #490cf5;
  border: solid 1px #afa2fa;
  background-color: #f9f0ff;
`;

export const ResolvidoList = styled.div`
  border-radius: 4px;
  display: inline-grid;
  padding-right: 20px;
  padding-left: 20px;
  justify-items: center;
  color: #d06d12;
  background-color: #fff2e8;
  border: solid 1px #eec4ad;
`;
export const AprovadoList = styled.div`
  border-radius: 4px;
  display: inline-grid;
  padding-right: 20px;
  padding-left: 20px;
  justify-items: center;
  color: #198459;
  border: solid 1px #90ceb5;
  background-color: #f6ffed;
`;

export const PendenteList = styled.div`
  border-radius: 4px;
  display: inline-grid;
  padding-right: 20px;
  padding-left: 20px;
  justify-items: center;
  color: #490cf5;
  border: solid 1px #afa2fa;
  background-color: #f9f0ff;
`;

export const Campo = styled.div`
  .ant-input:focus {
    border-color: #d9d9d9 !important;
    -webkit-box-shadow: none !important;
    box-shadow: none !important;
  }

  .ant-input:hover {
    border-color: #d9d9d9 !important;
    -webkit-box-shadow: none !important;
    box-shadow: none !important;
  }
`;

export const CampoDescricao = styled(TextArea)`
  font-family: Roboto;
  font-size: 14px !important;
  font-weight: normal !important;
  font-stretch: normal !important;
  font-style: normal !important;
  line-height: 1.57 !important;
  letter-spacing: normal !important;
  color: #42474a;
`;
