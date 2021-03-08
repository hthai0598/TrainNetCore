import React from 'react'
import { Link } from 'react-router-dom'
import { inject, observer } from 'mobx-react'
import logo from '../../assets/imgs/otrafy-logo-white-login@2x.png'
import LoadingSpinner from '../elements/LoadingSpinner'
import {
  AuthLayoutWrapper,
  LogoWrapper,
  Form,
  AuthFooter
} from './CustomLayoutStyled'

const AuthLayout = ({ children, authStore, commonStore }) => {
  return (
    <AuthLayoutWrapper style={{
      background: `${commonStore.appTheme.gradientColor}`,
    }}>
      <LogoWrapper>
        <img src={logo} alt="Otrafy"/>
      </LogoWrapper>
      <Form>
        {children}
      </Form>
      <AuthFooter>
        <p>&copy; 2019 - Otrafy Technology</p>
        <ul>
          <li>
            <Link to="/terms-of-service">Terms of service</Link>
          </li>
          <li>
            <Link to="/privacy-policy">Privacy policy</Link>
          </li>
        </ul>
      </AuthFooter>
      {
        commonStore.isAppLoading
          ? <LoadingSpinner theme={commonStore.appTheme} type={'page'}/>
          : null
      }
    </AuthLayoutWrapper>
  )
}

export default inject('authStore', 'commonStore')(observer(AuthLayout))