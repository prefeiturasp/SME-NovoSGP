import React, { useEffect } from 'react';
import styled from 'styled-components';
import { exibirAlerta } from '~/servicos/alertas';
import tipoPermissao from '~/dtos/tipoPermissao';
import { useSelector } from 'react-redux';

const CardPrincipal = props => {
  const { children, rota } = props;
  const Corpo = styled.div`
    padding:0;
    margin:0;
  `;
  const usuario = useSelector(store => store.usuario);
  useEffect(() => verificaSomenteConsulta(), [])

  const verificaSomenteConsulta = () => {
    const permissoes = usuario.permissoes[rota];
    if (permissoes && permissoes[tipoPermissao.podeConsultar] && !permissoes[tipoPermissao.podeAlterar]
      && !permissoes[tipoPermissao.podeIncluir] && !permissoes[tipoPermissao.podeExcluir]) {
      exibirAlerta('warning', 'Você tem apenas permissão de consulta nesta tela')
    }
  }

  return (
    <Corpo>
      {children}
    </Corpo>
  )
}

export default CardPrincipal;
