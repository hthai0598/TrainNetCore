import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import AuthLayout from '../../layout/AuthLayout'
import LoginForm from './LoginForm'

const LoginPage = () => {

  useEffect(() => {
    if (localStorage.getItem('jwt')) {
      history.push('/')
    }
  }, [])

  return (
    <AuthLayout>
      <Helmet>
        <title>Login | Otrafy</title>
      </Helmet>
      <LoginForm/>
    </AuthLayout>
  )
}

export default LoginPage