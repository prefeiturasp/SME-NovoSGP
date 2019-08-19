import React, { Component } from 'react';
import ReactQuill from 'react-quill';

export default class TesteEditor extends Component {
  constructor(props) {
    super(props);
    const { text } = props;
    this.state = { text };
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(value) {
    this.setState({ text: value });
  }

  render() {
    const { text } = this.state;
    return <ReactQuill value={text} onChange={this.handleChange} />;
  }
}
