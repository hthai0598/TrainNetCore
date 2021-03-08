import React from 'react'
import { Helmet } from 'react-helmet'
import AuthLayout from '../../layout/AuthLayout'
import emailIcon from '../../../assets/icons/email-icon@2x.png'
import {
  IconWrapper,
  Heading,
  Message,
  NaviLink,
} from './CustomStyled'

const ForgotPasswordSentSuccessPage = () => {
  return (
    <AuthLayout>
      <Helmet>
        <title>Email sent successfully | Otrafy</title>
      </Helmet>
      <IconWrapper>
        <img src={emailIcon} alt="Email"/>
      </IconWrapper>
      <Heading>Reset password</Heading>
      <Message>A reset password link has been sent to your email. Please click on the link to reset your
        password.</Message>
      <NaviLink to={'/login'} className={'color-link'}>
        Back to login
      </NaviLink>
    </AuthLayout>
  )
}

export default ForgotPasswordSentSuccessPage