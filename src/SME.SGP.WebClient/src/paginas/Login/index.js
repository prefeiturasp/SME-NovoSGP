import React, { useEffect } from 'react';
import LoginController from './controller';

export default function Login() {


    useEffect(() =>{

      console.log(LoginController.enviarDados({usuario: "danieli.amcom",senha: "Sgp1234"}));

    })

  return <h5>Teste 123</h5>;
}
