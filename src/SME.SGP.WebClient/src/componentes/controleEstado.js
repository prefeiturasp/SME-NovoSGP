import React from 'react';
import { Prompt } from 'react-router-dom';
import { confirmacao } from '../servicos/alertas';

export class ControleEstado extends React.Component {
  state = {
    confirmou: false,
    mostrarModal: true,
  };

  fecharModal = callback =>
    this.setState(
      {
        mostrarModal: false,
      },
      callback
    );

  bloquearNavegacao = nextLocation => {
    const { confirmou } = this.state;
    const { cancelar } = this.props;

    if (!confirmou) {
      confirmacao(
        'Atenção',
        'Você não salvou as informações preenchidas, Deseja realmente cancelar as alterações?',
        () => this.acaoConfirmar(nextLocation),
        cancelar
      );
      return false;
    }
  };

  acaoConfirmar = path => {
    this.fecharModal(() => {
      const { confirmar } = this.props;
      const { lastLocation } = this.state;
      this.setState(
        {
          confirmou: true,
        },
        () => {
          confirmar(path.pathname);
        }
      );
    });
  };

  render() {
    const { when } = this.props;
    return (
      <>
        <Prompt when={when} message={this.bloquearNavegacao} />
      </>
    );
  }
}
export default ControleEstado;
