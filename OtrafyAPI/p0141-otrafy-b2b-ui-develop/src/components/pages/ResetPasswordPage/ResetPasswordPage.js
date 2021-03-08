import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import { inject, observer } from 'mobx-react'
import AuthLayout from '../../layout/AuthLayout'
import ResetPasswordForm from './ResetPasswordForm'

const ResetPasswordPage = ({ authStore, history, match }) => {

  const resetPasswordToken = match.params.token

  useEffect(() => {
    authStore.checkResetPasswordToken(resetPasswordToken, history)
  }, [])

  const handleResetPassword = password =>
    authStore.resetPassword(resetPasswordToken, password, history)

  return (
    <AuthLayout>
      <Helmet>
        <title>Reset password | Otrafy</title>
      </Helmet>
      <ResetPasswordForm
        onReset={password => handleResetPassword(password)}/>
    </AuthLayout>
  )
}

export default inject('authStore')(observer(ResetPasswordPage))