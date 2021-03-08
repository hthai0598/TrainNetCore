import { observable, action, toJS } from 'mobx'
import { message } from 'antd'
// Store
import suppliersStore from './suppliersStore'
// Request
import { ProductRequest } from '../requests'

class ProductsStore {

  @observable isLoading = false
  @observable productsList = []
  @observable paging = {}
  @observable editMode = false
  @observable productInfo = {}

  @observable toggleEditMode(state) {
    this.editMode = state
  }

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action updateProductDetail(productId, info) {
    this.setLoadingProgress(true)
    ProductRequest.updateProductDetail(productId, info)
      .then(() => {
        this.getProductById(productId)
      })
      .then(() => {
        this.toggleEditMode(false)
        message.success(`Product detail updated successfully`)
      })
      .catch(error => {
        console.log(error)
        message.error(`An error occurred`)
      })
      .finally(() => this.setLoadingProgress(false))
  }

  @action getProductById(productId) {
    this.setLoadingProgress(true)
    ProductRequest.getProductById(productId)
      .then(response => {
        this.productInfo = response.data.result
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action getAllProducts(params) {
    this.setLoadingProgress(true)
    ProductRequest.getAllProducts(params)
      .then(response => {
        this.productsList = response.data.result.data
        this.paging = response.data.result.paging
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action createProduct(info, history) {
    this.setLoadingProgress(true)
    ProductRequest.createProduct(info)
      .then(() => suppliersStore.setActiveTab('3'))
      .then(() => {
        history.push(`/suppliers-management/suppliers-detail/${info.supplierId}`)
        message.success(`Product create successfully`)
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

}

export default new ProductsStore()