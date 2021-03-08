import { action, observable } from 'mobx'
import { message } from 'antd'
// Request
import { CompanyRequest } from '../requests'

class CompaniesStore {
  @observable isLoading = false
  @observable companiesList = []
  @observable currentCompanyView = {}
  @observable paging = {}
  @observable editMode = false
  @observable buyerInvitationDetail = {}
  
  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action setDefaultPaging() {
    this.paging = {}
  }

  @action setEditMode(state) {
    this.editMode = state
  }

  @action createCompany(submitValues, history) {
    this.setLoadingProgress(true)
    CompanyRequest.create(submitValues)
      .then(() => {
        message.success(`Company "${submitValues.name}" has been created`)
      })
      .then(() => history.push('/company-management'))
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getCompanyList(params) {
    this.setLoadingProgress(true)
    CompanyRequest.getAll(params)
      .then(response => {
        this.companiesList = response.data.result.data
        this.paging = response.data.result.paging
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getCurrentCompanyView(id, history) {
    this.setLoadingProgress(true)
    CompanyRequest.getById(id)
      .then(response => {
        this.currentCompanyView = response.data.result
      })
      .catch(() => {
        message.error(`Invalid URL`)
        history.push('/company-management')
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateCompanyInfo(id, info) {
    this.setLoadingProgress(true)
    CompanyRequest.updateInfo(id, info)
      .then(() => {
        message.success(`Company info updated successfully`)
        this.setEditMode(false)
        this.getCurrentCompanyView(id)
      })
      .catch(() => message.error(`An error occurred`))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getBuyerDetail(buyerId) {
    this.setLoadingProgress(true)
    CompanyRequest.getBuyerDetail(buyerId)
      .then(response => this.buyerInvitationDetail = response.data.result)
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateBuyerDetail(buyerId, submitValues) {
    this.setLoadingProgress(true)
    CompanyRequest.updateBuyerDetail(buyerId, submitValues)
      .then(response => {
        this.setLoadingProgress(false)
        message.success(`${response.data.message}`)
      })
      .then(() => this.getBuyerDetail(buyerId))
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }
}

export default new CompaniesStore()