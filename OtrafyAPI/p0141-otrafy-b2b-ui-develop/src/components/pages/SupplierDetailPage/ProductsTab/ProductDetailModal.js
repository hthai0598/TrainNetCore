import React, { useEffect } from 'react'
import { Modal } from 'antd'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import RequestListTable from '../RequestListTable'
import ProductDetailCard from './ProductDetailCard'
import ProductDetailEditCard from './ProductDetailEditCard'
import {
  ModalContentWrapper,
} from './ProductsTabStyled'

const ProductDetailModal = ({ visible, handleClose, productsStore, tagsStore }) => {
  useEffect(() => {
    tagsStore.getAllTags('')
  }, [])
  return (
    <Modal centered
           footer={false}
           onCancel={handleClose}
           visible={visible}>
      <ModalContentWrapper>
        <div className="col-left">
          {
            productsStore.editMode
              ? <ProductDetailEditCard tagsList={tagsStore.tagsList}/>
              : <ProductDetailCard/>
          }
        </div>
        <div className="col-right">
          <RequestListTable/>
        </div>
      </ModalContentWrapper>
    </Modal>
  )
}

export default withRouter(inject('productsStore', 'tagsStore')(observer(ProductDetailModal)))