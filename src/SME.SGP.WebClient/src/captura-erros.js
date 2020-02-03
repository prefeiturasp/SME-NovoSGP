import React, { Component } from 'react';
import history from '~/servicos/history';

export default class CapturaErros extends Component {
  state = { has_error: false };

  componentDidCatch(error, info) {
    this.setState({ has_error: true });
    history.push('/erro');
  }

  render() {
    return this.props.children;
  }
}
