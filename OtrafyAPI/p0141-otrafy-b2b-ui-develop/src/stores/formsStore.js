import { observable, action, toJS } from 'mobx'
import { message } from 'antd'
import uuid from 'uuid'
// Stores
import buyersStore from './buyersStore'
// Request
import { FormRequest } from '../requests'

class FormsStore {

  @observable isLoading = false
  @observable pageCount = 1
  @observable itemCountPerPage = 0
  @observable currentPageForm = 1
  @observable draggedItemFromBasicFields = {}
  @observable selectedElementIndex = undefined
  @observable submittedFormBuilder = {
    showProgressBar: 'top',
    pages: [
      {
        name: 'page1',
        elements: [],
      },
    ],
  }
  @observable paging = {}
  @observable formsList = []

  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  // Form data
  @action getFormDetail(formId) {
    this.setLoadingProgress(true)
    FormRequest.getFormDetail(formId)
      .then(response => {
        const formDetail = JSON.parse(response.data.result.formDesignerData[0].surveyDesigner)[0]
        this.submittedFormBuilder.pages = formDetail.pages
        this.pageCount = JSON.parse(response.data.result.formDesignerData[0].surveyDesigner)[0].pages.length
        buyersStore.updateFormCreateValues('name', response.data.result.name)
        buyersStore.updateFormCreateValues('description', response.data.result.description)
        buyersStore.updateFormCreateValues('tags', response.data.result.tags)
        buyersStore.updateFormCreateValues('surveyDesigner', JSON.parse(response.data.result.formDesignerData[0].surveyDesigner))
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  @action setDefaultPaging() {
    this.paging = {}
  }

  @action getFormsList(params) {
    this.setLoadingProgress(true)
    FormRequest.getAllForms(params)
      .then(response => {
        this.formsList = response.data.result.data
        this.paging = response.data.result.paging
      })
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

  // Form builder
  @action setElementPosition(pos) {
    this.submittedFormBuilder.pages[this.currentPageForm - 1].elements = pos
  }

  @action checkBlankPage() {
    const formPage = [...this.submittedFormBuilder.pages]
    for (let i = 0; i < formPage.length; i++) {
      if (formPage[i].elements.length === 0) {
        message.error(`Require at least 1 item on page!`)
        this.selectedElementIndex = undefined
        this.currentPageForm = i + 1
        return false
      }
    }
    return true
  }

  @action clearAllFormsData() {
    this.pageCount = 1
    this.itemCountPerPage = 0
    this.currentPageForm = 1
    this.draggedItemFromBasicFields = {}
    this.selectedElementIndex = undefined
    this.submittedFormBuilder = {
      showProgressBar: 'top',
      pages: [
        {
          name: 'page1',
          elements: [],
        },
      ],
    }
  }

  @action updateElementProperties(key, value) {
    const currentPage = this.currentPageForm - 1
    const elementIndex = this.selectedElementIndex
    const currentElement = this.submittedFormBuilder.pages[currentPage].elements[elementIndex]
    currentElement[key] = value
  }

  @action showElementProperties(elemIndex) {
    this.selectedElementIndex = elemIndex
  }

  @action deletePage(index) {
    this.pageCount--
    this.currentPageForm = 1
    this.submittedFormBuilder.pages.splice(index, 1)
    message.success(`Page deleted`)
  }

  @action pageNavigation(direction) {
    this.selectedElementIndex = undefined
    switch (direction) {
      case 'back':
        this.currentPageForm--
        break
      case 'next':
        this.currentPageForm++
        break
    }
  }

  @action addPage() {
    this.selectedElementIndex = undefined
    this.pageCount++
    this.currentPageForm++
    message.success(`Page added!`)
    this.submittedFormBuilder.pages.push({
      name: `page${this.pageCount}`,
      elements: [],
    })
  }

  @action selectItemFromBasicFields(item) {
    this.draggedItemFromBasicFields = item
  }

  @action addItemToFormPage(item) {
    let temp = {
      ...item,
      name: uuid(),
    }
    this.itemCountPerPage++
    this.draggedItemFromBasicFields = {}
    this.submittedFormBuilder.pages[this.currentPageForm - 1].elements.push(temp)
  }

  @action deleteElementFromPage(index) {
    this.selectedElementIndex = undefined
    this.submittedFormBuilder.pages[this.currentPageForm - 1].elements.splice(index, 1)
  }

}

export default new FormsStore()