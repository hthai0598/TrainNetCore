import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import AuthLayout from '../../layout/AuthLayout'
import ForgotPasswordForm from './ForgotPasswordForm'

const ForgotPasswordPage = ({ history }) => {

  useEffect(() => {
    if (localStorage.getItem('jwt')) {
      history.push('/')
    }
  }, [])

  return (
    <AuthLayout>
      <Helmet>
        <title>Forgot password | Otrafy</title>
      </Helmet>
      <ForgotPasswordForm/>
    </AuthLayout>
  )
}

export default ForgotPasswordPage