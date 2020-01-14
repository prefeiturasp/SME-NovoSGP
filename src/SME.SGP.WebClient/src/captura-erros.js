import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import history from '~/servicos/history';

export default class CapturaErros extends Component {
  state = { has_error: false }

  componentDidCatch(error, info) {
    this.setState({ has_error: true });
    history.push("/erro");
  }


  render() {
    // if (this.state.has_error) {
    //   return <Redirect to="/erro" />
    // }
    return this.props.children;
  }

};
