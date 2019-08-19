import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Base, Active, Hover } from './colors';


const Alert = props => {  
    const {
        tipo,
    } = props;

  return (
<div 
  class={`alert alert-${tipo} alert-dismissible fade show`} role="alert">
  {`${props.children}`}
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>);

};


export default Alert;
