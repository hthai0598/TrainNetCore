import { observable, action } from 'mobx'
import { message } from 'antd'
// Request
import { SupplierRequest } from '../requests'

class SuppliersStore {

  @observable isLoading = false
  @observable suppliersList = []
  @observable paging = {}
  @observable editMode = false
  @observable supplierDetail = {}
  @observable supplierDetailActiveTab = '1'
  @observable addSupplierFormValues = {
    companyName: '',
    firstName: '',
    lastName: '',
    username: '',
    email: '',
  }

  @action clearForm() {
    this.addSupplierFormValues = {
      companyName: '',
      firstName: '',
      lastName: '',
      username: '',
      email: '',
    }
  }

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action setActiveTab(key) {
    this.supplierDetailActiveTab = key
  }

  @action updateSupplierDetail(supplierId, newInfo) {
    this.setLoadingProgress(true)
    SupplierRequest.updateSupplierDetail(supplierId, newInfo)
      .then(response => {
        this.toggleEditMode(false)
        message.success(response.data.message)
      })
      .catch(error => {
        message.error(error.data.message)
        this.setLoadingProgress(false)
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action toggleEditMode(state) {
    this.editMode = state
  }

  @action getSupplierDetail(supplierId) {
    this.setLoadingProgress(true)
    SupplierRequest.getSupplierDetail(supplierId)
      .then(response => {
        this.supplierDetail = response.data.result
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action updateSelectedTags(value) {
    this.addSupplierFormValues.selectedTags.push(value)
  }

  @action updateAddSupplierFormValues(key, val) {
    this.addSupplierFormValues[key] = val
  }

  @action createSupplier(value, history) {
    this.setLoadingProgress(true)
    SupplierRequest.create(value)
      .then(response => {
        message.success(response.data.message)
        this.setLoadingProgress(false)
        this.clearForm()
      })
      .then(() => history.push('/suppliers-management'))
      .catch(error => {
        this.setLoadingProgress(false)
        message.error(error.data.message)
      })
  }

  @action getAllSuppliers(params) {
    this.setLoadingProgress(true)
    SupplierRequest.getAll(params)
      .then(response => {
        this.paging = response.data.result.paging
        this.suppliersList = response.data.result.data
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

}

export default new SuppliersStore()