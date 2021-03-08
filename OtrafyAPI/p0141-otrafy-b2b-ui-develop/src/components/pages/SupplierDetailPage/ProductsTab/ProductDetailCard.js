import React from 'react'
import { inject, observer } from 'mobx-react'
import { Tooltip, Icon } from 'antd'
import {
  ProductDetailCardWrapper,
} from './ProductsTabStyled'
import NormalTag from '../../../elements/NormalTag'
// Icons
import { ReactComponent as DotsIcon } from '../../../../assets/svg/dots-icn.svg'
import { ReactComponent as ChartIcon } from '../../../../assets/svg/chart-icn.svg'
import { ReactComponent as NoteIcon } from '../../../../assets/svg/note-icn.svg'
import { ReactComponent as TagIcon } from '../../../../assets/svg/tag-icn.svg'

const ProductDetailCard = ({ productsStore, commonStore }) => {
  return (
    <ProductDetailCardWrapper>
      <div className="heading">
        <div className={'info'}>
          {productsStore.productInfo.name}
        </div>
        <Tooltip title={'Edit product detail'}>
          <div className="action"
               style={{ background: commonStore.appTheme.gradientColor }}
               onClick={() => productsStore.toggleEditMode(true)}>
            <Icon type="edit"/>
          </div>
        </Tooltip>
      </div>
      <div className="body">
        <dl className="list">
          <dt>
            <DotsIcon className={'color-svg'} style={{ width: 9.33 }}/>
            ID:
          </dt>
          <dd>
            {productsStore.productInfo.code}
          </dd>
          <dt>
            <ChartIcon className={'color-svg'} style={{ width: 10.5 }}/>
            Grade:
          </dt>
          <dd>
            {productsStore.productInfo.grade}
          </dd>
          <dt>
            <NoteIcon className={'color-svg'} style={{ width: 11.67 }}/>
            Description:
          </dt>
          <dd>
            {productsStore.productInfo.description}
          </dd>
          <dt>
            <TagIcon className={'color-svg'} style={{ width: 11.67 }}/>
            Tags:
          </dt>
          <dd>
            {
              productsStore.productInfo.tags
                ? <NormalTag tags={productsStore.productInfo.tags}/>
                : null
            }
          </dd>
        </dl>
      </div>
    </ProductDetailCardWrapper>
  )
}

export default inject('productsStore', 'commonStore')(observer(ProductDetailCard))