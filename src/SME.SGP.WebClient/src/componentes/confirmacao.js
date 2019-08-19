import { Modal } from 'antd';

const { confirm } = Modal;

function Confirmacao({ texto, titulo, confirmar, cancelar }) {
  confirm({
    title: titulo,
    content: texto,
    onOk() {
      confirmar();
    },
    onCancel() {
      cancelar();
    },
  });
  return <></>;
}
