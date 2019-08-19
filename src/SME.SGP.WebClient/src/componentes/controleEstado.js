import React from 'react';
import { Prompt } from 'react-router-dom';
import { confirmacao } from '../servicos/alertas';

export class ControleEstado extends React.Component {
  state = {
    modalVisible: false,
    lastLocation: null,
    confirmedNavigation: false,
  };

  showModal = location =>
    this.setState({
      modalVisible: true,
      lastLocation: location,
    });

  closeModal = callback =>
    this.setState(
      {
        modalVisible: false,
      },
      callback
    );

  handleBlockedNavigation = nextLocation => {
    const { confirmedNavigation } = this.state;
    const { shouldBlockNavigation } = this.props;
    if (!confirmedNavigation && shouldBlockNavigation(nextLocation)) {
      confirmacao(
        'Atenção',
        'Você não salvou as informações preenchidas,<br> <b>Deseja realmente cancelar as alterações?</b>',
        () => this.handleConfirmNavigationClick(),
        this.props.cancelar
      );
      return false;
    }

    return true;
  };

  handleConfirmNavigationClick = () => {
    this.closeModal(() => {
      const { confirmar, navigate } = this.props;
      const { lastLocation } = this.state;
      this.setState(
        {
          confirmedNavigation: true,
        },
        () => {
          // Navigate to the previous blocked location with your navigate function
          navigate();
        }
      );
    });
  };

  render() {
    const { when } = this.props;
    const { modalVisible, lastLocation } = this.state;
    return (
      <>
        <Prompt when={when} message={this.handleBlockedNavigation} />
        {/* {modalVisible ? <h1>bloqueou</h1> : <h1>Não</h1>} */}
      </>
    );
  }
}
export default ControleEstado;
