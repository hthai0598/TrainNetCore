import { observable, action, toJS } from 'mobx'
// Request
import { TagRequest } from '../requests'

class TagsStore {

  @observable isLoading = false
  @observable tagsList = []

  @action clearTags() {
    this.tagsList = []
  }
  
  @action setLoadingProgress(state) {
    this.isLoading = state
  }

  @action checkAndCreateNewTag(tagArr) {
    if (tagArr) {
      let tagsList = [...toJS(this.tagsList)]
      let removedDuplicateEntryArr = tagArr.filter(tag => tagsList.indexOf(tag) === -1)
      let removedBlankTag = removedDuplicateEntryArr.filter(el => el.trim())
      if (removedBlankTag.length === 0) return
      TagRequest.updateTags(removedBlankTag)
        .catch(error => console.log(error))
    }
  }

  @action getAllTags(params) {
    this.setLoadingProgress(true)
    TagRequest.getAll(params)
      .then(response => this.tagsList = response.data.result)
      .catch(error => console.log(error))
      .finally(() => this.setLoadingProgress(false))
  }

}

export default new TagsStore()