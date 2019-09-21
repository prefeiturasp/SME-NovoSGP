import React, { useState } from 'react'
import styled from 'styled-components';
import { Base } from '../componentes/colors';

const BtnPerfil = () => {
  const [perfilSelecionado, setPerfilSelecionado] = useState({
    id: 4,
    descricao: 'Professor Orientador de Área',
    abreviacao: 'POA'
  });

  const perfis = {
    perfilSelecionado: {
      id: 4,
      descricao: 'Professor Orientador de Área',
      abreviacao: 'POA'
    },
    data: [
      {
        id: 1,
        descricao: 'Diretor',
      },
      {
        id: 2,
        descricao: 'Professor'
      },
      {
        id: 3,
        descricao: 'Coordenador Pedagógico',
        abreviacao: 'CP'
      },
      {
        id: 4,
        descricao: 'Professor Orientador de Área',
        abreviacao: 'POA'
      }
    ]
  };
  const Perfil = styled.div`
    margin: 0 5px 0 5px;
    align-items: center !important;
  `;

  const IconePerfil = styled.a`
    padding-bottom: 10px;

    i{
      justify-content: center !important;
      display: flex !important;
      background: ${Base.Roxo} !important;
      border-radius: 50% !important;
      color: ${Base.Branco} !important;
      font-size: 18px !important;
      height: 28px !important;
      width: 28px !important;
    }
    .btn{
      background: ${Base.Roxo};
      color: ${Base.Branco};
    }
  `
  const ItensPerfil = styled.ul`
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    width:145px;
    height: auto;
    background: ${Base.Branco};
    border: solid ${Base.CinzaDesabilitado} 1px;
    position: absolute;
    right:100px;
  `;

  const Item = styled.div`  
    text-align: left;
    width: 100%;
    height:100%;
    vertical-align: middle !important;

    &:not(:last-child){
      border-bottom: solid ${Base.CinzaDesabilitado} 1px !important;
    }

    &:hover{
      cursor: pointer;
    }
    
    li{
      height: 35px;
      list-style-type: none;
      font-size: 10px;
      padding-left: 7px;
      vertical-align: middle;
      display: table-cell;
      width: 145px;
    }

    i{
      padding-right: 7px;
      font-size: 14px;
      color: #707683;
    }

    li:hover{
      background: #e7e6f8;
      font-weight: bold !important;      
    }
  `;

  const gravarPerfilSelecionado = (perfil) => {
    if (perfil.value) {
      const perfilNovo = perfis.data.filter(item => item.id === perfil.value)
      setPerfilSelecionado(perfilNovo[0]);
    }
  }


  return (
    <Perfil>
      <IconePerfil>
        <i className="fas fa-user-circle btn fa-lg mb-1"></i>
      </IconePerfil>
      {perfilSelecionado.abreviacao ? perfilSelecionado.abreviacao : perfilSelecionado.descricao}
      <ItensPerfil className="list-inline">
        {perfis.data.map(item =>
          <Item key={item.id} onClick={(e) => gravarPerfilSelecionado(e.target)}>
            <li style={{ fontWeight: item.id === perfilSelecionado.id ? 'bold' : 'initial' }} value={item.id}>
              <i value={item.id} className="fas fa-user-circle"></i>
              {item.descricao + (item.abreviacao ? "(" + item.abreviacao + ")" : "")}
            </li>
          </Item>
        )}
      </ItensPerfil>
    </Perfil>
  )
}

export default BtnPerfil;
