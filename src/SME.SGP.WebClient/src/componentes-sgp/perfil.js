import React, { useState, useEffect } from 'react'
import styled from 'styled-components';
import { Base } from '../componentes/colors';
import history from '../servicos/history';
import { store } from '../redux';
import { perfilSelecionado, setarPerfis } from '../redux/modulos/perfil/actions';
import { useSelector } from 'react-redux';
import api from '../servicos/api';


const Perfil = () => {
  const [ocultaPerfis, setarOcultaPerfis] = useState(true);
  const PerfilStore = useSelector(store => store.perfil);

  const buscarPerfis = () => {
    if (!PerfilStore.perfilSelecionado.id) {
      api.get(
        `/v1/perfis`
      ).then(result => {
        if (result.data) {
          const perfisNovos = result.data.perfis;
          const selecionado = perfisNovos.filter(p => p.id === result.data.perfilSelecionado);
          store.dispatch(perfilSelecionado(selecionado[0]));
          store.dispatch(setarPerfis(perfisNovos));
        }
      })
    }
  };

  //Ajustar com a conslusão do login
  useEffect(() => buscarPerfis(), []);

  // const perfis = {
  //   perfilSelecionado: {
  //     id: "2",
  //     descricao: 'Professor'
  //   },
  //   dados: [
  //     {
  //       id: "1",
  //       descricao: 'Diretor',
  //     },
  //     {
  //       id: "2",
  //       descricao: 'Professor'
  //     },
  //     {
  //       id: "3",
  //       descricao: 'Coordenador Pedagógico',
  //       sigla: 'CP'
  //     },
  //     {
  //       id: "4",
  //       descricao: 'Professor Orientador de Área',
  //       sigla: 'POA'
  //     }
  //   ]
  // };

  const ItensPerfil = styled.div`
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    width:145px;
    height: auto;
    background: ${Base.Branco};
    border: solid ${Base.CinzaDesabilitado} 1px;
    position: absolute;
    right:16%;
  `;

  const Item = styled.tr`
    text-align: left;
    width: 100%;
    height:100%;
    vertical-align: middle !important;

    &:not(:last-child){
      border-bottom: solid ${Base.CinzaDesabilitado} 1px !important;
    }

    &:hover{
      cursor: pointer;
      background: #e7e6f8;
      font-weight: bold !important;
    }

    td{
      height: 35px;
      font-size: 10px;
      padding-left: 7px;
      width: 145px;
    }

    i{
      font-size: 14px;
      color: #707683;
    }
  `;

  const Botao = styled.a`
    display: block !important;
    text-align: center !important;
  `;

  const IconePerfil = styled.div`
    background: ${Base.Roxo};
    color: ${Base.Branco};
    font-size: 18px !important;
    height: 28px !important;
    width: 28px !important;
    vertical-align: middle;
    box-sizing: border-box;
    align-items: center !important;
    border-radius: 50%;
    display: inline-block;
    justify-content: center !important;

    i{
      margin-top:5px;
    }
  `;

  const gravarPerfilSelecionado = (perfil) => {
    if (perfil) {
      const perfilNovo = PerfilStore.perfis.filter(item => item.id === perfil)
      store.dispatch(perfilSelecionado(perfilNovo[0]));
      setarOcultaPerfis(true);
      if (PerfilStore.perfilSelecionado.id !== perfilNovo[0].id) {
        history.push('/');
      }
    }
  }

  const onClickBotao = () => {
    if (PerfilStore.perfis.length > 1) {
      setarOcultaPerfis(!ocultaPerfis);
    }
  };


  return (
    <div className="position-relative">
      <Botao className="text-center" onClick={onClickBotao} style={{ cursor: PerfilStore.perfis.length > 1 ? 'pointer' : 'default' }}>
        <IconePerfil>
          <i className="fas fa-user-circle" />
        </IconePerfil>
        <span className={`d-block mt-1 ${ocultaPerfis ? '' : ' font-weight-bold'}`} >
          {PerfilStore.perfilSelecionado.sigla ? PerfilStore.perfilSelecionado.sigla : PerfilStore.perfilSelecionado.descricao}
        </span>
      </Botao>
      <ItensPerfil hidden={ocultaPerfis} className="list-inline">
        <table>
          <tbody>
            {PerfilStore.perfis.map(item =>
              <Item key={item.id}
                onClick={(e) => gravarPerfilSelecionado(e.currentTarget.accessKey)}
                accessKey={item.id}>
                <td style={{ width: '20px' }}>
                  <i value={item.id} className="fas fa-user-circle"></i>
                </td>
                <td style={{ width: '100%', fontWeight: item.id === PerfilStore.perfilSelecionado.id ? 'bold' : 'initial' }}>
                  {item.descricao + (item.sigla ? "(" + item.sigla + ")" : "")}
                </td>
              </Item>
            )}
          </tbody>
        </table>
      </ItensPerfil>
    </div>
  )
}

export default Perfil;
