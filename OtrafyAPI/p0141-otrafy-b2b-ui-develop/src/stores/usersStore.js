import { observable, action, toJS } from 'mobx'
import { message } from 'antd'
import { apiUrl } from '../config'
import { UserRequest } from '../requests'
import commonStore from './commonStore'

class UsersStore {

  @observable currentUser = {}
  @observable editMode = false
  @observable isLoading = false
  
  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action updateUserInfo(newInfo) {
    this.setLoadingProgress(true)
    UserRequest.updateProfiles(newInfo)
      .then(response => {
        message.success(response.data.message)
        this.editMode = false
        this.setCurrentUser()
      })
      .catch(() => message.error(`An error occurred`))
      .finally(() => this.setLoadingProgress(false))
  }

  @action userLogout() {
    this.currentUser = {}
    commonStore.clearToken()
    return Promise.resolve()
  }

  @action toggleEditMode(state) {
    this.editMode = state
  }

  @action setCurrentUser(history) {
    this.setLoadingProgress(true)
    if (commonStore.checkToken()) {
      UserRequest.getCurrent()
        .then(response => {
          this.currentUser = response.data.result
        })
        .catch(() => {
          commonStore.clearToken()
          history ? history.push('/login') : null
        })
        .finally(() => this.setLoadingProgress(false))
    } else {
      this.setLoadingProgress(false)
    }
  }

}

export default new UsersStore()
