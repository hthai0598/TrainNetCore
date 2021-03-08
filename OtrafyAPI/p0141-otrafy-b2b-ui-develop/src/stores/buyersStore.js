import { observable, action, toJS } from 'mobx'
import { message } from 'antd'
import moment from 'moment'
// Stores
import commonStore from './commonStore'
import usersStore from './usersStore'
import formsStore from './formsStore'
import tagsStore from './tagsStore'
// Request
import { BuyerRequest } from '../requests'
import { FormRequest } from '../requests'

class BuyersStore {
  @observable isLoading = false
  @observable buyerList = []
  @observable statistic = {}
  @observable paging = {}
  @observable formCreateProgress = 1
  @observable formCreateValues = {
    formType: 0,
    name: undefined,
    description: undefined,
    tags: undefined,
    surveyDesigner: undefined,
  }

  @action clearForm() {
    this.formCreateValues = {
      formType: 0,
      name: undefined,
      description: undefined,
      tags: undefined,
      surveyDesigner: undefined,
    }
  }

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action getBuyerStatistic() {
    this.setLoadingProgress(true)
    BuyerRequest.getStatistic()
      .then(response => {
        this.statistic = response.data.result
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateFormData(formId, history) {
    if (!formsStore.checkBlankPage()) return
    this.formCreateValues.surveyDesigner = [toJS(formsStore.submittedFormBuilder)]
    this.setLoadingProgress(true)
    tagsStore.checkAndCreateNewTag(this.formCreateValues.tags)
    FormRequest.updateFormDetail(formId, toJS(this.formCreateValues))
      .then(() => {
        message.success(`Form updated successfully`)
        history.push('/all-forms')
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action createFormData(history) {
    if (!formsStore.checkBlankPage()) return
    this.formCreateValues.surveyDesigner = [toJS(formsStore.submittedFormBuilder)]
    this.setLoadingProgress(true)
    tagsStore.checkAndCreateNewTag(this.formCreateValues.tags)
    FormRequest.createForm(toJS(this.formCreateValues))
      .then(() => {
        message.success(`Form create successfully`)
        history.push('/all-forms')
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateFormCreateValues(key, val) {
    this.formCreateValues[key] = val
  }

  @action updateSelectedTag(val) {
    this.formCreateValues.tags.push(val)
  }

  @action setFormCreationProgress(step) {
    this.formCreateProgress = step
  }

  @action getCurrentBuyerList(params) {
    this.setLoadingProgress(true)
    BuyerRequest.getAll(params)
      .then(response => {
        this.paging = response.data.result.paging
        this.buyerList = response.data.result.data
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action resendInvitation(buyerId) {
    this.setLoadingProgress(true)
    BuyerRequest.resendInvitation(buyerId)
      .then(() => {
        this.setLoadingProgress(false)
        message.success(`Invitation sent successfully`)
      })
      .catch(error => message.error(error.data.message))
      .finally(() => this.setLoadingProgress(false))
  }

  @action createBuyerInvitation(values, history) {
    this.setLoadingProgress(true)
    BuyerRequest.createBuyerInvitation(values)
      .then(() => {
        message.success(`An email has been sent to buyer`)
        history.push(`/company-management/${values.companyId}`)
      })
      .catch(error => {
        message.error(error.data.message)
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action activeBuyer(submitValue, history) {
    this.setLoadingProgress(true)
    BuyerRequest.activeBuyer(submitValue)
      .then(response => {
        console.log(response)
        message.success(`Account has been activated`)
        commonStore.setToken(response.data.result.accessToken)
        commonStore.setTokenExpiration(moment(response.data.result.tokenExpiration)._d)
        localStorage.setItem('jwt', response.data.result.accessToken)
        localStorage.setItem('tokenExpiration', moment(response.data.result.tokenExpiration)._d)
      })
      .then(() => usersStore.setCurrentUser())
      .then(() => {
        history.push('/my-profile')
        usersStore.toggleEditMode(true)
      })
      .catch(error => {
        console.log(error)
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action checkValidBuyerInviteToken(token, history) {
    usersStore.userLogout()
    this.setLoadingProgress(true)
    BuyerRequest.validateToken(token)
      .catch(() => {
        message.error(`Invalid or expired token`)
        history.push('/login')
      })
      .finally(() => this.setLoadingProgress(false))
  }
}

export default new BuyersStore()