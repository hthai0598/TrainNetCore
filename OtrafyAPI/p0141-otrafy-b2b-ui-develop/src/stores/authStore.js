import { observable, action } from 'mobx'
import moment from 'moment'
import { message } from 'antd'
// Stores
import commonStore from './commonStore'
import usersStore from './usersStore'
// Requests
import { AuthRequest } from '../requests'

class AuthStore {
  @observable errors = undefined
  @observable isLoading = false

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action userLogin(username, password, remember, history) {
    this.setLoadingProgress(true)
    AuthRequest.login(username, password)
      .then(response => {
        commonStore.setRemember(remember)
        commonStore.setToken(response.data.result.accessToken)
        commonStore.setTokenExpiration(moment(response.data.result.tokenExpiration)._d)
        if (commonStore.isRemember) {
          localStorage.setItem('jwt', response.data.result.accessToken)
          localStorage.setItem('tokenExpiration', moment(response.data.result.tokenExpiration)._d)
        } else {
          sessionStorage.setItem('jwt', response.data.result.accessToken)
        }
        usersStore.setCurrentUser(history)
        return response.data.result
      })
      .then(data => {
        if (history.location.state) {
          history.push(`${history.location.state.from.pathname}`)
        } else {
          history.push('/')
        }
        message.success(`Welcome, ${data.firstName} ${data.lastName}`)
      })
      .catch(error => {
        message.error(error.data.message)
      })
      .finally(() => {
        this.setLoadingProgress(false)
      })
  }

  @action userForgotPassword(email, history) {
    this.setLoadingProgress(true)
    AuthRequest.forgetPassword(email)
      .then(() => {
        history.push('/forgot-password/success')
      })
      .catch(error => {
        message.error(error.data.message)
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action checkResetPasswordToken(token, history) {
    this.setLoadingProgress(true)
    AuthRequest.checkResetPasswordToken(token)
      .catch(error => {
        message.error(error.data.message)
        history.push('/forgot-password')
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action resetPassword(token, newPassword, history) {
    this.setLoadingProgress(true)
    AuthRequest.changePassword(token, newPassword)
      .then(() => {
        message.success(`Your password has been changed successfully`, 5)
        commonStore.clearToken()
      })
      .then(() => history.push('/login'))
      .catch(error => {
        message.error(error.data.message)
      })
      .finally(() => this.setLoadingProgress(false))
  }
}

export default new AuthStore()
