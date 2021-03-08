import React from 'react'
import PropTypes from 'prop-types'
import { inject, observer } from 'mobx-react'
import { Icon, Pagination } from 'antd'
import {
  Wrapper, Jumper,
} from './CustomStyled'

const CustomPagination = ({ pageCount, total, current, pageSize, onChange, hideOnSinglePage, commonStore }) => {

  const itemRender = (current, type, originalElement) => {
    switch (type) {
      case 'prev':
        return <Icon type="caret-left"/>
      case 'next':
        return <Icon type="caret-right"/>
      default:
        return originalElement
    }
  }

  return (
    <Wrapper>
      {
        pageCount <= 1
          ? null
          : <Jumper allowed={current !== 1}
                    theme={commonStore.appTheme}
                    onClick={() => current !== 1 ? onChange(1) : null}>
            <Icon type="step-backward"/>
          </Jumper>
      }
      <Pagination
        itemRender={itemRender}
        defaultCurrent={1} total={total}
        current={current} pageSize={pageSize}
        hideOnSinglePage={hideOnSinglePage}
        onChange={page => onChange(page)}/>
      {
        pageCount <= 1
          ? null
          : <Jumper allowed={current !== pageCount}
                    theme={commonStore.appTheme}
                    onClick={() => current !== pageCount ? onChange(pageCount) : null}>
            <Icon type="step-forward"/>
          </Jumper>
      }
    </Wrapper>
  )
}

CustomPagination.propTypes = {
  pageCount: PropTypes.number,
  total: PropTypes.number,
  current: PropTypes.number,
  pageSize: PropTypes.number,
  onChange: PropTypes.func,
  hideOnSinglePage: PropTypes.bool,
}

export default inject('commonStore')(observer(CustomPagination))