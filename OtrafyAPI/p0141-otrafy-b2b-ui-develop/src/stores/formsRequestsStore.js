import { observable, action } from 'mobx'
import { toJS } from 'mobx'
import { message } from 'antd'
// Request
import { FormRequestRequest } from '../requests'

class FormsRequestsStore {

  @observable formsRequestsList = []
  @observable paging = {}
  @observable isLoading = false
  @observable formRequestData = {
    title: undefined,
    description: undefined,
    selectedSupplierId: undefined,
    selectedProductId: undefined,
  }
  @observable formRequestDetail = {}
  @observable formRequestComments = []
  @observable survey = {
    currentPage: 0,
    currentForm: 0,
    surveyData: [],
    surveyResult: [],
  }

  @action clearFormRequestData() {
    this.formRequestData = {
      title: undefined,
      description: undefined,
      selectedSupplierId: undefined,
      selectedProductId: undefined,
    }
  }

  @action changeSurveyResult(key, val) {
    const { currentForm } = this.survey
    this.survey.surveyResult[currentForm][key] = val
  }

  @action goToNextRequestPage() {
    this.survey.currentPage++
  }

  @action goToPrevRequestPage() {
    this.survey.currentPage--
  }

  @action getFormRequestDetailById(formRequestId) {
    this.setLoadingProgress(true)
    FormRequestRequest.getFormRequestDetail(formRequestId)
      .then(response => {
        this.formRequestDetail = response.data.result
        this.survey.surveyData = response.data.result.formRequestDataDetail
        for (let i = 0; i < response.data.result.formRequestDataDetail.length; i++) {
          this.survey.surveyResult.push({})
        }
        console.log('survey data', response.data.result.formRequestDataDetail)
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action clearOnLeave() {
    this.formRequestDetail = {}
    this.formRequestComments = []
    this.survey = {
      currentPage: 0,
      currentForm: 0,
      surveyData: [],
      surveyResult: [],
    }
  }

  @action addComment(formRequestId, comment, sortType) {
    this.setLoadingProgress(true)
    FormRequestRequest.addComment(formRequestId, comment)
      .then(() => this.getRequestComments(`?formrequestId=${formRequestId}&SortType=${sortType}`))
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getRequestComments(params) {
    this.setLoadingProgress(true)
    FormRequestRequest.getAllRequestComments(params)
      .then(response => {
        this.formRequestComments = response.data.result
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action createRequest(info, history) {
    this.setLoadingProgress(true)
    FormRequestRequest.createRequest(info)
      .then(() => {
        this.clearFormRequestData()
        message.success(`Request created successfully`)
      })
      .then(() => history.push(`/suppliers-management`))
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateFormRequestData(key, val) {
    this.formRequestData[key] = val
  }

  @action getFormsRequestsList(params) {
    this.setLoadingProgress(true)
    FormRequestRequest.getRequestList(params)
      .then(response => {
        this.formsRequestsList = response.data.result.data
        this.paging = response.data.result.paging
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action deleteRequest(supplierId, formRequestId) {
    this.setLoadingProgress(true)
    FormRequestRequest.deleteRequest(supplierId, formRequestId)
      .then(() => {
        this.getFormsRequestsList(`?pageSize=10&pageNumber=1&supplierId=${supplierId}`)
      })
      .then(() => message.success(`Request deleted successfully`))
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getFormsRequestForSupplier(params) {
    this.setLoadingProgress(true)
    FormRequestRequest.getAllSupplierFormRequest(params)
      .then(response => {
        this.formsRequestsList = response.data.result.data
        this.paging = response.data.result.paging
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }
}

export default new FormsRequestsStore()